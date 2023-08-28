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
public class WorldShard
{
    public string shardName;
    public BoundsInt? bounds;
    public Coordinates position;
    public bool wasGenerated
    {
        get { return !String.IsNullOrEmpty(shardName); }
    }
    public bool isDisplayed
    {
        get { return bounds != null; }
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

public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private string seed;
    [SerializeField]
    public StageSO Stage;
    [SerializeField]
    private Grid grid;

    private WorldEditor _worldEditor;

    public Tilemap FloorTilemap;

    List<ShardSave> shardSaveList;

    private Dictionary<Coordinates, WorldShard> _shardMap;
    private Dictionary<string, TileBase> _refTiles;

    private WorldShard currentShard; 

    // Start is called before the first frame update
    void Start()
    {
        seed = this.GenerateSeed();
        UnityEngine.Random.InitState(seed.GetHashCode());

        _worldEditor = GetComponent<WorldEditor>();
        _shardMap = new Dictionary<Coordinates, WorldShard>();
        _refTiles = Resources.LoadAll<TileBase>("Tiles/" + Stage.TilesFolderName).ToDictionary(x => x.name, x => x);

        shardSaveList = _worldEditor.GetAllShardSave(Stage);
        InitMap();
        gameObject.GetComponent<EnemySpawnerController>().Floor = FloorTilemap;
        gameObject.GetComponent<KeyController>().Floor = FloorTilemap;
    }

    private void InitMap()
    {
        int randomPick = UnityEngine.Random.Range(0, shardSaveList.Count);
        ShardSave shardSave = shardSaveList[randomPick];

        foreach (LayerSave tileLayer in shardSave.tileLayers)
        {
            Tilemap tm = grid.transform.Find(tileLayer.name).GetComponent<Tilemap>();
            foreach (TileSave tile in tileLayer.tileList)
            {
                tm.SetTile(tile.pos, _refTiles[tile.tileName]);
            }
            /*TileBase[] tilesToCopy = tileLayer.tileList.Select(t => _refTiles[t.tileName]).ToArray();
            tm.SetTilesBlock(tileLayer.bounds, tilesToCopy);*/


            if (tm.name.Equals("Floor"))
            {
                Coordinates origin = new Coordinates(0, 0);
                currentShard = new WorldShard()
                {
                    shardName = shardSave.saveName,
                    bounds = shardSave.shardBounds,
                    position = origin
                };
                FloorTilemap = tm;
                _shardMap.Add(origin, currentShard);
            }
        }
        PopoulateNeighbours(currentShard);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameManager.GameState.Player;
        if (!player) return;

        EdgePosition nearEdge = GetEdgePosition(player.transform.position, currentShard.bounds);
        if (nearEdge.Equals(EdgePosition.None)) return;

        ClearTiles(currentShard, nearEdge);
        Vector2Int orientation = GetEdgeLocation(nearEdge);
        Coordinates checkCoordinates = new Coordinates(currentShard.position.val + orientation);

        currentShard = _shardMap[checkCoordinates];
        PopoulateNeighbours(currentShard);
    }

    private IEnumerator PasteOnLayer(Tilemap tm, WorldShard shard, LayerSave tileLayer, ShardSave shardSave, Vector2Int orientation, Coordinates checkCoordinates)
    {
        bool wasAlreadyGenerated = _shardMap.ContainsKey(checkCoordinates) && _shardMap[checkCoordinates].wasGenerated;

        Vector3Int position = shard.position.val3 * shardSave.shardBounds.size; //shard.bounds.Value.position;

        Vector3Int offset = new Vector3Int(
            Mathf.RoundToInt((orientation.x * shardSave.shardBounds.size.x) + position.x),
            Mathf.RoundToInt((orientation.y * shardSave.shardBounds.size.y) + position.y)
        );

        Debug.DrawLine(offset, position, Color.cyan, 15);

        TileBase[] tilesToCopy = tileLayer.tileList.Select(t => _refTiles[t.tileName]).ToArray();
        BoundsInt bounds = CopyTilesWithOffset(tm, tileLayer, shardSave, offset);
        DrawBounds(bounds, Color.yellow, 15);

        if (tm.name.Equals("Floor"))
        {
            WorldShard newShard = new WorldShard()
            {
                shardName = shardSave.saveName,
                bounds = bounds,
                position = checkCoordinates
            };
            // If was already generated only add to it's coordinates
            if (wasAlreadyGenerated)
            {
                _shardMap[checkCoordinates] = newShard;
            }
            else
            {
                _shardMap.Add(checkCoordinates, newShard);
            }
        }
        yield return null;
    }

    private IEnumerator PopulateIndicatedNeighbour(WorldShard shard, Coordinates checkCoordinates, Vector2Int orientation)
    {
        bool wasAlreadyGenerated = _shardMap.ContainsKey(checkCoordinates) && _shardMap[checkCoordinates].wasGenerated;

        // If was already generated then use it's name
        ShardSave shardSave;
        if (wasAlreadyGenerated)
        {
            shardSave = shardSaveList.Where(x => x.saveName.Equals(_shardMap[checkCoordinates].shardName)).FirstOrDefault();
        }
        else
        {
            int randomPick = UnityEngine.Random.Range(0, shardSaveList.Count);
            shardSave = shardSaveList[randomPick];
        }

        Debug.Log("LOADING AT " + checkCoordinates.val + " - " + _shardMap.ContainsKey(checkCoordinates) + " / " + wasAlreadyGenerated);
        foreach (LayerSave tileLayer in shardSave.tileLayers)
        {
            if (!shard.bounds.HasValue) continue;
            Tilemap tm = grid.transform.Find(tileLayer.name).GetComponent<Tilemap>();
            StartCoroutine(PasteOnLayer(tm, shard, tileLayer, shardSave, orientation, checkCoordinates));
        }
        yield return null;
    }

    private void PopoulateNeighbours(WorldShard shard)
    {
        Debug.Log("POPULATE NEIGBHOURS FROM " + shard.position);
        foreach (EdgePosition edge in Enum.GetValues(typeof(EdgePosition)))
        {
            if (edge.Equals(EdgePosition.None)) continue;
            Vector2Int orientation = GetEdgeLocation(edge);
            Coordinates checkCoordinates = new Coordinates(shard.position.val + orientation);
            if (_shardMap.ContainsKey(checkCoordinates) && _shardMap[checkCoordinates].isDisplayed) continue;
            StartCoroutine(PopulateIndicatedNeighbour(shard, checkCoordinates, orientation));
        }
    }

    private IEnumerator ClearIndicatedTiles(WorldShard shard, EdgePosition furthestEdge)
    {
        Vector2Int orientation = GetEdgeLocation(furthestEdge);
        Coordinates checkCoordinates = new Coordinates(shard.position.val + orientation);
        if (_shardMap.ContainsKey(checkCoordinates) && _shardMap[checkCoordinates] != null)
        {
            if (!_shardMap[checkCoordinates].bounds.HasValue) yield return true;
            BoundsInt bounds = _shardMap[checkCoordinates].bounds.Value;
            foreach (Transform child in grid.transform)
            {
                Tilemap tm = child.GetComponent<Tilemap>();
                TileBase[] emptyTiles = new TileBase[bounds.size.x * bounds.size.y];
                tm.SetTilesBlock(bounds, emptyTiles);
            }
            // When clearing shard data only delete bounds, not other metadata
            _shardMap[checkCoordinates].bounds = null;
        }
        yield return null;
    }

    private void ClearTiles(WorldShard shard, EdgePosition nearEdge)
    {
        List<EdgePosition> furthestEdges = GetFurthestEdges(nearEdge);
        foreach (EdgePosition furthestEdge in furthestEdges)
        {
            StartCoroutine(ClearIndicatedTiles(shard, furthestEdge));
        }
    }

    private BoundsInt CopyTilesWithOffset(Tilemap targetTilemap, LayerSave tileLayer, ShardSave shardSave, Vector3Int offset)
    {
        TileBase[] tilesToCopy = tileLayer.tileList.Select(t => _refTiles[t.tileName]).ToArray();
        BoundsInt bounds = new BoundsInt(
            Mathf.FloorToInt(offset.x - shardSave.shardBounds.size.x / 2),
            Mathf.FloorToInt(offset.y - shardSave.shardBounds.size.y / 2), 0,
            shardSave.shardBounds.size.x,
            shardSave.shardBounds.size.y, 1
        );
        foreach (TileSave tile in tileLayer.tileList)
        {
            targetTilemap.SetTile(tile.pos + offset, _refTiles[tile.tileName]);
        }
        //targetTilemap.SetTilesBlock(bounds, tilesToCopy);
        return bounds;
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
