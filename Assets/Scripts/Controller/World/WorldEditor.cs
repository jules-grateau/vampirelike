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
        _defaultBounds = new BoundsInt(-25, -25, 0, 50, 50, 1);
    }


    public void SaveShard(StageSO stage, string saveName)
    {
        string savePath = Application.dataPath + "/Resources/Stages/" + stage.name;
        bool exists = System.IO.Directory.Exists(savePath);
        if (!exists) {
            System.IO.Directory.CreateDirectory(savePath);
        }

        ShardSave export = new ShardSave(saveName);
        export.shardBounds = _defaultBounds;

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

    public void LoadShard(StageSO stage, string saveName)
    {
        TextAsset shard = Resources.Load<TextAsset>("Stages/" + stage.name + "/" + saveName);
        ShardSave shardSave = JsonUtility.FromJson<ShardSave>(shard.text);

        foreach (LayerSave tileLayer in shardSave.tileLayers)
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

    public List<ShardSave> GetAllShardSave(StageSO stage)
    {
        TextAsset[] shard = Resources.LoadAll<TextAsset>("Stages/" + stage.name);
        return shard.Select( s => JsonUtility.FromJson<ShardSave>(s.text)).ToList();
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
