using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui.Stats
{
    public class InGamePlayerStatListController : MonoBehaviour
    {
        private void Awake()
        {
            if (GameManager.GameState == null) return;

            StatListController statListController = gameObject.AddComponent<StatListController>();
            GameObject player = GameManager.GameState.Player;
            if (!player) return;
            BaseStatistics<CharacterStatisticEnum> characterStatistics = player.GetComponent<PlayerStatsController>().CharacterStatistics;
            BaseStatistics<WeaponStatisticEnum> weaponStatistics = player.GetComponent<WeaponInventoryManager>().WeaponStats;

            statListController.Init(characterStatistics, weaponStatistics);
        }
    }
}