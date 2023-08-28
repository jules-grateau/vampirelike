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


[ExecuteInEditMode]
public class WorldEditor : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    private BoundsInt _defaultBounds;

    // Start is called before the first frame update
    void Start()
    {
        // Default Chunk Size
        _defaultBounds = new BoundsInt(-25, -25, 0, 50, 50, 1);
    }


    public void SaveChunk(StageSO stage, string saveName)
    {
        string savePath = Application.dataPath + "/Resources/Stages/" + stage.name;
        bool exists = System.IO.Directory.Exists(savePath);
        if (!exists) {
            System.IO.Directory.CreateDirectory(savePath);
        }

        ChunkSave export = new ChunkSave(saveName);
        export.chunkBounds = _defaultBounds;

        foreach (Transform child in grid.transform)
        {
            bool isCollider = child.GetComponent<TilemapCollider2D>();
            LayerSave ls = new LayerSave(child.name, isCollider);

            Tilemap tm = child.GetComponent<Tilemap>();
            tm.CompressBounds();

            ls.tileList = tm.GetTilesBlock(_defaultBounds)
                .Select( (t, i) => new TileSave(i, t, _defaultBounds.size.x))
                .Where(t => !String.IsNullOrEmpty(t.tileName))
                .ToList();

            export.tileLayers.Add(ls);
        }

        string json = JsonUtility.ToJson(export);
        File.WriteAllText(savePath + "/" + saveName + ".json", json);
        Debug.Log("SAVED AT : " + savePath + "/" + saveName + ".json");
    }

    public void LoadChunk(StageSO stage, string saveName)
    {
        TextAsset chunk = Resources.Load<TextAsset>("Stages/" + stage.name + "/" + saveName);
        ChunkSave chunkSave = JsonUtility.FromJson<ChunkSave>(chunk.text);

        foreach (LayerSave tileLayer in chunkSave.tileLayers)
        {
            Tilemap tm = grid.transform.Find(tileLayer.name).GetComponent<Tilemap>();
            //TileBase[] tilesToCopy = tileLayer.tileList.Select(t => Resources.Load<TileBase>("Tiles/" + stage.TilesFolderName + "/" + t.tileName)).ToArray();
            //tm.SetTilesBlock(tileLayer.bounds, tilesToCopy);
            foreach (TileSave tile in tileLayer.tileList)
            {
                tm.SetTile(tile.pos, Resources.Load<TileBase>("Tiles/" + stage.TilesFolderName + "/" + tile.tileName));
            }
        }

        Debug.Log("LOADED FROM : " + saveName + ".json");
    }

    public List<ChunkSave> GetAllChunkSave(StageSO stage)
    {
        TextAsset[] chunk = Resources.LoadAll<TextAsset>("Stages/" + stage.name);
        return chunk.Select( s => JsonUtility.FromJson<ChunkSave>(s.text)).ToList();
    }

    public void ClearAllTiles()
    {
        foreach (Transform child in grid.transform)
        {
            Tilemap tm = child.GetComponent<Tilemap>();
            tm.ClearAllTiles();
        }
    }
}
