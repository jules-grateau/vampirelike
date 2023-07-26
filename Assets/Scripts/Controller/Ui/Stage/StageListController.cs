using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Stage;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class StageListController : MonoBehaviour
    {
        GameObject _characterInfoPrefab;
        StageSO[] __stages;
        // Use this for initialization
        void Awake()
        {
            _characterInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/StageInfo");
            __stages = Resources.LoadAll<StageSO>("ScriptableObjects/Stage/");

            foreach(StageSO stage in __stages)
            {
                GameObject characterInfo = Instantiate(_characterInfoPrefab, transform);
                StageInfoController stageInfoController = characterInfo.GetComponent<StageInfoController>();
                stageInfoController.Init(stage);
            }
        }
    }
}