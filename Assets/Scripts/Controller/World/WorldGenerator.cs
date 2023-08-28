using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Controller.Game;
using System;
using System.IO;
using UnityEngine.Tilemaps;
using Assets.Scripts.Controller.Enemies;
using Assets.Scripts.ScriptableObjects.Stage;
using System.Threading;


[Serializable]
public class WorldChunk
{
    public string chunkName;
    public Coordinates position;
    public BoundsInt? bounds;

    [System.NonSerialized]
    public GameObject gameObject;
    public bool wasGenerated
    {
        get { return !String.IsNullOrEmpty(chunkName); }
    }
    public bool isDisplayed
    {
        get { return gameObject != null; }
    }
}

public class Coordinates
{
    public Vector2Int val;
    public Vector3Int val3 {
        get { return new Vector3Int(val.x, val.y, 0); }
    }

    public Coordinates(int x, int y)
    {
        val = new Vector2Int(x, y);
    }

    public Coordinates(Vector2Int pos)
    {
        val = pos;
    }

    public override bool Equals(object o)
    {
        return 
            (o as Coordinates)?.val.x == val.x &&
            (o as Coordinates)?.val.y == val.y;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }
}

public enum EdgePosition
{
    None,
    Left,
    Right,
    Top,
    Bottom,
    UpperLeftCorner,
    UpperRightCorner,
    LowerLeftCorner,
    LowerRightCorner
}

public class PoolElement<T>
{
    public T Element;
}

public class Pool<T>
{
    private Queue<PoolElement<T>> _pool;

    public Pool()
    {
        _pool = new Queue<PoolElement<T>>();
    }

    public void Add(T pe)
    {
        _pool.Enqueue(new PoolElement<T>() { Element = pe });
    }

    public void Set(List<T> toPool)
    {
        _pool = new Queue<PoolElement<T>>(toPool.Select(p => new PoolElement<T>() { Element = p }).ToList());
    }

    public T Take()
    {
        PoolElement<T> e = _pool.Dequeue();
        return e.Element;
    }

    public void Release(T element)
    {
        _pool.Enqueue(new PoolElement<T>() { Element = element });
    }
}

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private string seed;
    [SerializeField]
    public StageSO Stage;
    [SerializeField]
    private Grid grid;

    public Tilemap FloorTilemap;

    private Dictionary<Coordinates, WorldChunk> _chunkMap;
    private WorldChunk currentChunk;
    private List<string> _chunkNames;
    private Dictionary<string, Pool<GameObject>> _chunkPool;
    private int _poolSize = 9;
    private BoundsInt _defaultBounds = new BoundsInt(-25, -25, 0, 50, 50, 1);

    // Start is called before the first frame update
    void Start()
    {
        seed = this.GenerateSeed();
        UnityEngine.Random.InitState(seed.GetHashCode());

        _chunkMap = new Dictionary<Coordinates, WorldChunk>();
        _chunkNames = new List<string>();
        _chunkPool = new Dictionary<string, Pool<GameObject>>();

        List<GameObject> chunkGOList = Resources.LoadAll<GameObject>("Prefabs/Chunks/" + Stage.SceneName).ToList();
        foreach (GameObject chunkGO in chunkGOList)
        {
            Pool<GameObject> pool = new Pool<GameObject>();
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject poolChunk = Instantiate(chunkGO, grid.transform);
                poolChunk.SetActive(false);
                pool.Add(poolChunk);
            }
            _chunkNames.Add(chunkGO.name);
            _chunkPool.Add(chunkGO.name, pool);
        }

        InitMap();
    }

    public bool IsOnFloor(Vector3 pos)
    {
        Coordinates chunkCoords = PosToCoordinates(pos);
        foreach (Transform child in _chunkMap[chunkCoords].gameObject.transform)
        {
            Tilemap tm = child.GetComponent<Tilemap>();
            if (tm.name.Equals("Floor"))
            {
                Debug.Log(pos + " -> " + chunkCoords.val);
                return tm.HasTile(Vector3Int.FloorToInt(pos - (chunkCoords.val3 * _defaultBounds.size)));
            }
        }
        return false;
    }

    public Coordinates PosToCoordinates(Vector3 pos)
    {
        int x = Mathf.FloorToInt((pos.x + (_defaultBounds.size.x / 2)) / _defaultBounds.size.x);
        int y = Mathf.FloorToInt((pos.y + (_defaultBounds.size.y / 2)) / _defaultBounds.size.y);
        return new Coordinates(x, y);
    }

    private void InitMap()
    {
        int randomPick = UnityEngine.Random.Range(0, _chunkNames.Count);
        string chunkName = _chunkNames[randomPick];
        GameObject chunkFromPool = _chunkPool[chunkName].Take();
        chunkFromPool.SetActive(false);

        Coordinates origin = new Coordinates(0, 0);
        currentChunk = new WorldChunk()
        {
            chunkName = chunkName,
            bounds = _defaultBounds,
            position = origin,
            gameObject = chunkFromPool
        };
        chunkFromPool.transform.position = Vector3Int.zero;
        //FloorTilemap = tm;
        _chunkMap.Add(origin, currentChunk);
        chunkFromPool.SetActive(true);

        PopoulateNeighbours(currentChunk);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameManager.GameState.Player;
        if (!player) return;

        EdgePosition nearEdge = GetEdgePosition(player.transform.position, currentChunk.bounds);
        if (nearEdge.Equals(EdgePosition.None)) return;

        ClearTiles(currentChunk, nearEdge);
        Vector2Int orientation = GetEdgeLocation(nearEdge);
        Coordinates checkCoordinates = new Coordinates(currentChunk.position.val + orientation);

        currentChunk = _chunkMap[checkCoordinates];
        PopoulateNeighbours(currentChunk);
    }

    private IEnumerator PopulateIndicatedNeighbour(WorldChunk chunk, Coordinates checkCoordinates, Vector2Int orientation)
    {
        bool wasAlreadyGenerated = _chunkMap.ContainsKey(checkCoordinates) && _chunkMap[checkCoordinates].wasGenerated;

        // If was already generated then use it's name
        GameObject chunkFromPool;
        string chunkName;
        if (wasAlreadyGenerated)
        {
            chunkName = _chunkMap[checkCoordinates].chunkName;
        }
        else
        {
            int randomPick = UnityEngine.Random.Range(0, _chunkNames.Count);
            chunkName = _chunkNames[randomPick];
        }
        chunkFromPool = _chunkPool[chunkName].Take();

        Debug.Log("LOADING AT " + checkCoordinates.val + " - " + _chunkMap.ContainsKey(checkCoordinates) + " / " + wasAlreadyGenerated);

        if (!chunk.bounds.HasValue) yield return null;

        Vector3Int position = chunk.position.val3 * _defaultBounds.size; //chunk.bounds.Value.position;

        Vector3Int offset = new Vector3Int(
            Mathf.RoundToInt((orientation.x * _defaultBounds.size.x) + position.x),
            Mathf.RoundToInt((orientation.y * _defaultBounds.size.y) + position.y)
        );
        chunkFromPool.transform.position = offset;
        chunkFromPool.SetActive(true);

        Debug.DrawLine(offset, position, Color.cyan, 15);

        BoundsInt bounds = new BoundsInt(
            Mathf.FloorToInt(offset.x - _defaultBounds.size.x / 2),
            Mathf.FloorToInt(offset.y - _defaultBounds.size.y / 2), 0,
            _defaultBounds.size.x,
            _defaultBounds.size.y, 1
        );
        DrawBounds(bounds, Color.yellow, 15);

        WorldChunk newChunk = new WorldChunk()
        {
            chunkName = chunkName,
            bounds = bounds,
            position = checkCoordinates,
            gameObject = chunkFromPool
        };

        // If was already generated only add to it's coordinates
        if (wasAlreadyGenerated)
        {
            _chunkMap[checkCoordinates] = newChunk;
        }
        else
        {
            _chunkMap.Add(checkCoordinates, newChunk);
        }

        yield return null;
    }

    private void PopoulateNeighbours(WorldChunk chunk)
    {
        Debug.Log("POPULATE NEIGBHOURS FROM " + chunk.position);
        foreach (EdgePosition edge in Enum.GetValues(typeof(EdgePosition)))
        {
            if (edge.Equals(EdgePosition.None)) continue;
            Vector2Int orientation = GetEdgeLocation(edge);
            Coordinates checkCoordinates = new Coordinates(chunk.position.val + orientation);
            if (_chunkMap.ContainsKey(checkCoordinates) && _chunkMap[checkCoordinates].isDisplayed) continue;
            StartCoroutine(PopulateIndicatedNeighbour(chunk, checkCoordinates, orientation));
        }
    }

    private IEnumerator ClearIndicatedTiles(WorldChunk chunk, EdgePosition furthestEdge)
    {
        Vector2Int orientation = GetEdgeLocation(furthestEdge);
        Coordinates checkCoordinates = new Coordinates(chunk.position.val + orientation);
        WorldChunk sharAtCoordiantes = _chunkMap[checkCoordinates];
        if (_chunkMap.ContainsKey(checkCoordinates) && sharAtCoordiantes != null && sharAtCoordiantes.isDisplayed)
        {
            // When clearing chunk data only delete bounds, not other metadata
            sharAtCoordiantes.bounds = null;

            // Release to the pool
            sharAtCoordiantes.gameObject.SetActive(false);

            var k = _chunkPool[sharAtCoordiantes.chunkName];

            _chunkPool[sharAtCoordiantes.chunkName].Release(sharAtCoordiantes.gameObject);
            sharAtCoordiantes.gameObject = null;
        }
        yield return null;
    }

    private void ClearTiles(WorldChunk chunk, EdgePosition nearEdge)
    {
        List<EdgePosition> furthestEdges = GetFurthestEdges(nearEdge);
        foreach (EdgePosition furthestEdge in furthestEdges)
        {
            StartCoroutine(ClearIndicatedTiles(chunk, furthestEdge));
        }
    }

    private string GenerateSeed(int stringLength = 10)
    {
        int _stringLength = stringLength - 1;
        string randomString = "";
        string[] characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        for (int i = 0; i <= _stringLength; i++)
        {
            randomString = randomString + characters[UnityEngine.Random.Range(0, characters.Length)];
        }
        Debug.LogWarning("SEED = " + randomString);
        return randomString;
    }

    private Vector2Int GetEdgeLocation(EdgePosition enteredEdge)
    {
        switch (enteredEdge)
        {
            case EdgePosition.Left:
                return new Vector2Int(-1, 0);

            case EdgePosition.Right:
                return new Vector2Int(1, 0);

            case EdgePosition.Top:
                return new Vector2Int(0, 1);

            case EdgePosition.Bottom:
                return new Vector2Int(0, -1);

            case EdgePosition.UpperLeftCorner:
                return new Vector2Int(-1, 1);

            case EdgePosition.UpperRightCorner:
                return new Vector2Int(1, 1);

            case EdgePosition.LowerLeftCorner:
                return new Vector2Int(-1, -1);

            case EdgePosition.LowerRightCorner:
                return new Vector2Int(1, -1);

            case EdgePosition.None:
            default:
                return Vector2Int.zero; // Default to the center
        }
    }

    private List<EdgePosition> GetFurthestEdges(EdgePosition enteredEdge)
    {
        List<EdgePosition> furthestEdges = new List<EdgePosition>();

        switch (enteredEdge)
        {
            case EdgePosition.Left:
                furthestEdges.Add(EdgePosition.Right);
                furthestEdges.Add(EdgePosition.UpperRightCorner);
                furthestEdges.Add(EdgePosition.LowerRightCorner);
                break;

            case EdgePosition.Right:
                furthestEdges.Add(EdgePosition.Left);
                furthestEdges.Add(EdgePosition.UpperLeftCorner);
                furthestEdges.Add(EdgePosition.LowerLeftCorner);
                break;

            case EdgePosition.Top:
                furthestEdges.Add(EdgePosition.Bottom);
                furthestEdges.Add(EdgePosition.LowerLeftCorner);
                furthestEdges.Add(EdgePosition.LowerRightCorner);
                break;

            case EdgePosition.Bottom:
                furthestEdges.Add(EdgePosition.Top);
                furthestEdges.Add(EdgePosition.UpperLeftCorner);
                furthestEdges.Add(EdgePosition.UpperRightCorner);
                break;

            case EdgePosition.UpperLeftCorner:
                furthestEdges.Add(EdgePosition.UpperRightCorner);
                furthestEdges.Add(EdgePosition.Right);
                furthestEdges.Add(EdgePosition.LowerRightCorner);
                furthestEdges.Add(EdgePosition.Bottom);
                furthestEdges.Add(EdgePosition.LowerLeftCorner);
                break;

            case EdgePosition.UpperRightCorner:
                furthestEdges.Add(EdgePosition.UpperLeftCorner);
                furthestEdges.Add(EdgePosition.Left);
                furthestEdges.Add(EdgePosition.LowerLeftCorner);
                furthestEdges.Add(EdgePosition.Bottom);
                furthestEdges.Add(EdgePosition.LowerRightCorner);
                break;

            case EdgePosition.LowerLeftCorner:
                furthestEdges.Add(EdgePosition.UpperLeftCorner);
                furthestEdges.Add(EdgePosition.Top);
                furthestEdges.Add(EdgePosition.UpperRightCorner);
                furthestEdges.Add(EdgePosition.Right);
                furthestEdges.Add(EdgePosition.LowerRightCorner);
                break;

            case EdgePosition.LowerRightCorner:
                furthestEdges.Add(EdgePosition.UpperRightCorner);
                furthestEdges.Add(EdgePosition.Top);
                furthestEdges.Add(EdgePosition.UpperLeftCorner);
                furthestEdges.Add(EdgePosition.Left);
                furthestEdges.Add(EdgePosition.LowerLeftCorner);
                break;
            case EdgePosition.None:
                break;
        }

        return furthestEdges;
    }

    private EdgePosition GetEdgePosition(Vector2 targetPosition, BoundsInt? nullableCustomBounds)
    {
        if(!nullableCustomBounds.HasValue) return EdgePosition.None;
        BoundsInt customBounds = nullableCustomBounds.Value;

        bool isInsideBoundsX = targetPosition.x >= customBounds.min.x && targetPosition.x <= customBounds.max.x;
        bool isInsideBoundsY = targetPosition.y >= customBounds.min.y && targetPosition.y <= customBounds.max.y;

        DrawBounds(customBounds, Color.red, 0.1f);

        if (!isInsideBoundsX || !isInsideBoundsY)
        {
            bool isNearLeftEdge = targetPosition.x < customBounds.min.x;
            bool isNearRightEdge = targetPosition.x > customBounds.max.x;
            bool isNearTopEdge = targetPosition.y > customBounds.max.y;
            bool isNearBottomEdge = targetPosition.y < customBounds.min.y;

            if (isNearLeftEdge)
                return EdgePosition.Left;

            if (isNearRightEdge)
                return EdgePosition.Right;

            if (isNearTopEdge)
                return EdgePosition.Top;

            if (isNearBottomEdge)
                return EdgePosition.Bottom;
        }

        return EdgePosition.None;
    }

    void DrawBounds(BoundsInt bounds, Color color, float time)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        Vector3 p1 = new Vector3(min.x, min.y, min.z);
        Vector3 p2 = new Vector3(max.x, min.y, min.z);
        Vector3 p3 = new Vector3(max.x, min.y, max.z);
        Vector3 p4 = new Vector3(min.x, min.y, max.z);

        Vector3 p5 = new Vector3(min.x, max.y, min.z);
        Vector3 p6 = new Vector3(max.x, max.y, min.z);
        Vector3 p7 = new Vector3(max.x, max.y, max.z);
        Vector3 p8 = new Vector3(min.x, max.y, max.z);

        // Draw bottom edges
        Debug.DrawLine(p1, p2, color, time);
        Debug.DrawLine(p2, p3, color, time);
        Debug.DrawLine(p3, p4, color, time);
        Debug.DrawLine(p4, p1, color, time);

        // Draw top edges
        Debug.DrawLine(p5, p6, color, time);
        Debug.DrawLine(p6, p7, color, time);
        Debug.DrawLine(p7, p8, color, time);
        Debug.DrawLine(p8, p5, color, time);

        // Draw vertical edges connecting top and bottom
        Debug.DrawLine(p1, p5, color, time);
        Debug.DrawLine(p2, p6, color, time);
        Debug.DrawLine(p3, p7, color, time);
        Debug.DrawLine(p4, p8, color, time);
    }
}
