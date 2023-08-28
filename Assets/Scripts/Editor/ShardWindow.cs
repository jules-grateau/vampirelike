using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using Assets.Scripts.ScriptableObjects.Stage;

public class ChunkWindow : EditorWindow
{
    string m_ChunkName = "";
    StageSO[] m_Stages;
    int m_StageIndex = 0;
    string[] m_Chunks;
    int m_ChunkIndex = 0;
    WorldEditor m_worldGenerator;

    [MenuItem("Window/Chunk Editor")]
    static void Init()
    {

        ChunkWindow window = (ChunkWindow)EditorWindow.GetWindow(typeof(ChunkWindow));
        window.Show();
    }

    private void reloadStages()
    {
        m_worldGenerator = GameObject.FindObjectOfType<WorldEditor>();
        m_Stages = Resources.LoadAll<StageSO>("ScriptableObjects/Stage").ToArray();
    }

    public void reloadChunks()
    {
        TextAsset[] maps = Resources.LoadAll<TextAsset>("Stages/" + m_Stages[m_StageIndex].name);
        m_Chunks = maps.Select(x => x.name).ToArray();
    }


    void OnGUI()
    {
        reloadStages();
        reloadChunks();

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        this.m_StageIndex = EditorGUILayout.Popup(this.m_StageIndex, this.m_Stages.Select(x => x.name).ToArray());
        if (GUILayout.Button("Reload chunk from stage"))
        {
            reloadChunks();
        }

        this.m_ChunkIndex = EditorGUILayout.Popup(this.m_ChunkIndex, this.m_Chunks);

        if (GUILayout.Button("Load chunk"))
        {
            m_worldGenerator.LoadChunk(this.m_Stages[this.m_StageIndex], this.m_Chunks[this.m_ChunkIndex]);
            this.m_ChunkName = this.m_Chunks[this.m_ChunkIndex];
        }

        this.m_ChunkName = EditorGUILayout.TextField("Chunk Name", this.m_ChunkName);

        if (GUILayout.Button("Save chunk"))
        {
            m_worldGenerator.SaveChunk(this.m_Stages[this.m_StageIndex], this.m_ChunkName);
        }

        if (GUILayout.Button("Clear chunk"))
        {
            m_worldGenerator.ClearAllTiles();
            this.m_ChunkName = "";
        }
    }
}
