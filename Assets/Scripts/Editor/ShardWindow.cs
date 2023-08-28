using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using Assets.Scripts.ScriptableObjects.Stage;

public class ShardWindow : EditorWindow
{
    string m_ShardName = "";
    StageSO[] m_Stages;
    int m_StageIndex = 0;
    string[] m_Shards;
    int m_ShardIndex = 0;
    WorldEditor m_worldGenerator;

    [MenuItem("Window/Shard Editor")]
    static void Init()
    {

        ShardWindow window = (ShardWindow)EditorWindow.GetWindow(typeof(ShardWindow));
        window.Show();
    }

    private void reloadStages()
    {
        m_worldGenerator = GameObject.FindObjectOfType<WorldEditor>();
        m_Stages = Resources.LoadAll<StageSO>("ScriptableObjects/Stage").ToArray();
    }

    public void reloadShards()
    {
        TextAsset[] maps = Resources.LoadAll<TextAsset>("Stages/" + m_Stages[m_StageIndex].name);
        m_Shards = maps.Select(x => x.name).ToArray();
    }


    void OnGUI()
    {
        reloadStages();
        reloadShards();

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        this.m_StageIndex = EditorGUILayout.Popup(this.m_StageIndex, this.m_Stages.Select(x => x.name).ToArray());
        if (GUILayout.Button("Reload shard from stage"))
        {
            reloadShards();
        }

        this.m_ShardIndex = EditorGUILayout.Popup(this.m_ShardIndex, this.m_Shards);

        if (GUILayout.Button("Load shard"))
        {
            m_worldGenerator.LoadShard(this.m_Stages[this.m_StageIndex], this.m_Shards[this.m_ShardIndex]);
            this.m_ShardName = this.m_Shards[this.m_ShardIndex];
        }

        this.m_ShardName = EditorGUILayout.TextField("Shard Name", this.m_ShardName);

        if (GUILayout.Button("Save shard"))
        {
            m_worldGenerator.SaveShard(this.m_Stages[this.m_StageIndex], this.m_ShardName);
        }

        if (GUILayout.Button("Clear shard"))
        {
            m_worldGenerator.ClearAllTiles();
            this.m_ShardName = "";
        }
    }
}
