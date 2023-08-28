using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Controller.Game;
using System;
using UnityEngine.Tilemaps;
using Assets.Scripts.Controller.Enemies;

[Serializable]
public class TileSave
{
    public Vector3Int pos;
    public String tileName;
    public String fixedTileName;

    public TileSave(int i, TileBase tile, int rowSize)
    {
        this.pos = new Vector3Int(i % rowSize - (rowSize/2), i / rowSize - (rowSize/2));
        this.tileName = tile?.name ?? null;
        if (tile is RuleTile)
        {
            this.fixedTileName = ((RuleTile)tile)?.m_DefaultSprite?.name ?? null;
        }
        else
        {
            this.fixedTileName = tile?.name ?? null;
        }
    }
}

[Serializable]
public class LayerSave
{
    public string name;
    public bool isCollider = false;
    public List<TileSave> tileList;   

    public LayerSave(string n, bool c)
    {
        name = n;
        isCollider = c;
        tileList = new List<TileSave>();
    }
}

[Serializable]
public class ChunkSave
{
    public String saveName;
    public List<LayerSave> tileLayers;
    public BoundsInt chunkBounds;

    public ChunkSave(String name)
    {
        saveName = name;
        tileLayers = new List<LayerSave>();
    }
}
