using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controller.Ui
{
    public class StageListController : MonoBehaviour
    {
        GameObject _stageInfoPrefab;
        StageSO[] _stages;
        List<GameObject> _stageInfo;
        // Use this for initialization
        void Awake()
        {
            _stageInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/StageInfo");
            _stages = Resources.LoadAll<StageSO>("ScriptableObjects/Stage/");
            _stageInfo = new List<GameObject>();

            foreach(StageSO stage in _stages)
            {
                GameObject stageInfo = Instantiate(_stageInfoPrefab, transform);
                StageInfoController stageInfoController = stageInfo.GetComponent<StageInfoController>();
                stageInfoController.Init(stage);
                _stageInfo.Add(stageInfo);
            }
        }

        private void OnEnable()
        {
            if (_stageInfo.Count <= 0) return;

            EventSystem.current.SetSelectedGameObject(_stageInfo[0]);
        }
    }
}