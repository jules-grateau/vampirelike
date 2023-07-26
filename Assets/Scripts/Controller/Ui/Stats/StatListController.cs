using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.Controller.Ui.Stats;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Stage;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class StatListController : MonoBehaviour
    {
        void Awake()
        {
            GameObject statInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/StatInfo");
            StatisticSO[] stats = Resources.LoadAll<StatisticSO>("ScriptableObjects/PlayableCharacters/Statistics/");

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;

            PlayerStatsController playerStatsController = player.GetComponent<PlayerStatsController>();
            if (!playerStatsController) return;

            CharacterStatisticsSO characterStatistics = playerStatsController.CharacterStatistics;
            if (!characterStatistics) return;



            foreach(StatisticSO stat in stats)
            {
                GameObject statInfo = Instantiate(statInfoPrefab, transform);
                StatInfoController statInfoController = statInfo.GetComponent<StatInfoController>();
                statInfoController.Init(stat, characterStatistics.GetStats(stat.Key));
            }
        }
    }
}