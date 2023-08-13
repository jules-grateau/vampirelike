using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controller.Upgrades
{
    //Singleton with single instance per upgrade type
    public class UpgradeManager
    {
        //Singleton Part
        static Dictionary<string,UpgradeManager> _instances = new Dictionary<string, UpgradeManager>();
        Dictionary<UpgradeQuality, float> _qualityDropChance = new Dictionary<UpgradeQuality, float>
        {
            {UpgradeQuality.Common, 1f},
            {UpgradeQuality.Rare, 0.3f },
            {UpgradeQuality.Epic, 0.1f },
            {UpgradeQuality.Legendary, 0.01f}
        };
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

            List<UpgradeSO> previousUpgrades = playerStats.Upgrades.Select((upgrade) => (UpgradeSO) upgrade.UpgradeSO)
                .Concat(weaponManager.Upgrades.Select((upgrade) => (UpgradeSO) upgrade.UpgradeSO)).ToList();

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
                    return charactUpgrade.IsDropable(playerStats.CharacterStatistics.GetStats(charactUpgrade.StatsToUpgrade));

                })
               .Where(upgrade =>
               {
                   if (upgrade is not WeaponStatsUpgradeSO) return true;
                   WeaponStatsUpgradeSO weaponUpgrade = upgrade as WeaponStatsUpgradeSO;
                   return weaponUpgrade.IsDropable(weaponManager.WeaponStats.GetStats(weaponUpgrade.StatsToUpgrade));

               });


            return filteredUpgrades.ToList();
        }

        public List<Upgrade<UpgradeSO>> Draw(int nbToDraw)
        {
            List<UpgradeSO> upgrades = GetAvailableUpgrades();
            List<Upgrade<UpgradeSO>> upgradesToShow = new List<Upgrade<UpgradeSO>>();
            Dictionary<UpgradeQuality, List<UpgradeSO>> upgradesByQuality = new Dictionary<UpgradeQuality, List<UpgradeSO>>();

            foreach(UpgradeQuality quality in Enum.GetValues(typeof(UpgradeQuality)))
            {
                upgradesByQuality.Add(quality, upgrades.Where((upg) => upg.HasQuality(quality)).ToList());
            }


            while (upgradesToShow.Count < nbToDraw)
            {
                float randomQualityNumber = UnityEngine.Random.Range(0, 100) / 100f;
                UpgradeQuality[] qualityList = _qualityDropChance.Where(item => randomQualityNumber <= item.Value)
                    .OrderBy(item => item.Value)
                    .Select(item => item.Key).ToArray();

                int qualityIndex = 0;
                List<UpgradeSO> qualityRelatedUpgrades = null;
                UpgradeQuality quality = qualityList[0];

                while((qualityRelatedUpgrades  == null || qualityRelatedUpgrades.Count == 0) 
                    && qualityIndex < qualityList.Length)
                {
                    quality = qualityList[qualityIndex];
                    qualityRelatedUpgrades = upgradesByQuality.GetValueOrDefault(quality);
                    qualityIndex++;
                }

                if(qualityRelatedUpgrades == null || qualityRelatedUpgrades.Count == 0)
                {
                    //We found no upgrades
                    continue;
                }

                int randomUpgradeIndex = UnityEngine.Random.Range(0, qualityRelatedUpgrades.Count);
                UpgradeSO upgrade = qualityRelatedUpgrades[randomUpgradeIndex];

                upgradesToShow.Add(new Upgrade<UpgradeSO>(quality, upgrade));
                qualityRelatedUpgrades.RemoveAt(randomUpgradeIndex);
            }

            return upgradesToShow;
        }
    }
}