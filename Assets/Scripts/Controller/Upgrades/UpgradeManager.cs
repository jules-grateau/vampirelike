using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Upgrades
{
    //Singleton with single instance per upgrade type
    public class UpgradeManager
    {
        //Singleton Part
        static Dictionary<string,UpgradeManager> _instances = new Dictionary<string, UpgradeManager>();
        public static UpgradeManager GetInstance(string upgradeType)
        {
            if(_instances.GetValueOrDefault(upgradeType) == null)
            {
                UpgradeManager newInstance = new UpgradeManager(upgradeType);
                _instances.Add(upgradeType, newInstance);
                return newInstance;
            }

            return _instances.GetValueOrDefault(upgradeType);
        }

        List<UpgradeSO> _upgradeList;
        Dictionary<WeaponStatisticEnum,WeaponStatisticDescriptionSO> _weaponStatisticDescriptions;
        Dictionary<CharacterStatisticEnum,CharacterStatisticsDescriptionSO> _characterStatisticsDescriptions;

        private UpgradeManager(string upgradeType)
        {
            _upgradeList = new List<UpgradeSO>();
            foreach (string upgrade in upgradeType.Split(";"))
            {
                _upgradeList.AddRange(Resources.LoadAll<UpgradeSO>($"ScriptableObjects/Upgrade/{upgrade}"));
            }
            _weaponStatisticDescriptions = Resources.LoadAll<WeaponStatisticDescriptionSO>("ScriptableObjects/Weapons/Statistics")
                .ToDictionary(stat => stat.Key, stat => stat);
            _characterStatisticsDescriptions = Resources.LoadAll<CharacterStatisticsDescriptionSO>("ScriptableObjects/PlayableCharacters/Statistics").ToDictionary(stat => stat.Key, stat => stat);
        }

        public List<UpgradeSO> GetAvailableUpgrades()
        {
            return FilterUpgrades(_upgradeList);
        }

        List<UpgradeSO> FilterUpgrades(List<UpgradeSO> upgrades)
        {
            IEnumerable<UpgradeSO> filteredUpgrades = upgrades;
            WeaponInventoryManager weaponManager = GameManager.GameState.Player.GetComponent<WeaponInventoryManager>();
            PlayerStatsController playerStats = GameManager.GameState.Player.GetComponent<PlayerStatsController>();

            List<UpgradeSO> previousUpgrades = playerStats.Upgrades.Select((upgrade) => (UpgradeSO)upgrade)
                .Concat(weaponManager.Upgrades.Select((upgrade) => (UpgradeSO)upgrade)).ToList();

            //Filter out MaxAmount upgrades
            Dictionary<UpgradeSO, int> previousUpgradesDictionnary =
                previousUpgrades.GroupBy(upgrade => upgrade)
                .ToDictionary(group => group.Key, group => group.Count());

            filteredUpgrades = filteredUpgrades.Where(upgrade => upgrade.MaxAmount == 0 || upgrade.MaxAmount > previousUpgradesDictionnary.GetValueOrDefault(upgrade));

            //Filter weapon specific upgrades
            List<WeaponSO> weapons = weaponManager.Inventory.Select(weaponController => weaponController.weapon).ToList();
            filteredUpgrades = filteredUpgrades.Where(upgrade => upgrade is not SpecificWeaponStatsUpgradeSO || weapons.Contains((upgrade as SpecificWeaponStatsUpgradeSO).ForWeapon));

            //Filter stats when reaching max value
            filteredUpgrades = filteredUpgrades
                .Where(upgrade =>
                {
                    if (upgrade is not CharacterStatsUpgradeSO) return true;
                    CharacterStatsUpgradeSO charactUpgrade = upgrade as CharacterStatsUpgradeSO;
                    CharacterStatisticsDescriptionSO statsDescription = _characterStatisticsDescriptions.GetValueOrDefault(charactUpgrade.StatsToUpgrade);

                    if (statsDescription.MaxValue <= 0) return true;

                    if (playerStats.CharacterStatistics.GetStats(charactUpgrade.StatsToUpgrade) <= statsDescription.MaxValue) return true;

                    return false;

                })
               .Where(upgrade =>
               {
                   if (upgrade is not WeaponStatsUpgradeSO) return true;
                   WeaponStatsUpgradeSO weaponUpgrade = upgrade as WeaponStatsUpgradeSO;
                   WeaponStatisticDescriptionSO statsDescription = _weaponStatisticDescriptions.GetValueOrDefault(weaponUpgrade.StatsToUpgrade);
                   if (!statsDescription) return true;

                   if (statsDescription.MaxValue <= 0) return true;

                   if (weaponManager.WeaponStats.GetStats(weaponUpgrade.StatsToUpgrade) <= statsDescription.MaxValue) return true;

                   return false;

               });


            return filteredUpgrades.ToList();
        }
    }
}