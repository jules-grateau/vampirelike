using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.Controller.Ui.Stats;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using Assets.Scripts.ScriptableObjects.Stage;
using Assets.Scripts.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class StatListController : MonoBehaviour
    {
        public void Init(CharacterStatisticsSO characterStatistics, WeaponStatisticsSO weaponStatistics)
        {
            GameObject statInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/StatInfo");
            CharacterStatisticsDescriptionSO[] stats = Resources.LoadAll<CharacterStatisticsDescriptionSO>("ScriptableObjects/PlayableCharacters/Statistics/");
            WeaponStatisticDescriptionSO[] weaponStats = Resources.LoadAll<WeaponStatisticDescriptionSO>("ScriptableObjects/Weapons/Statistics/");

            foreach (CharacterStatisticsDescriptionSO stat in stats)
            {
                GameObject statInfo = Instantiate(statInfoPrefab, transform);
                StatInfoController statInfoController = statInfo.GetComponent<StatInfoController>();
                statInfoController.Init(stat.Name, characterStatistics.GetStats(stat.Key), stat.ValueAppendix);
            }

            foreach (WeaponStatisticDescriptionSO stat in weaponStats)
            {
                GameObject statInfo = Instantiate(statInfoPrefab, transform);
                StatInfoController statInfoController = statInfo.AddComponent<StatInfoController>();
                statInfoController.Init(stat.Name, weaponStatistics.GetStats(stat.Key), stat.ValueAppendix);
            }
        }
    }
}