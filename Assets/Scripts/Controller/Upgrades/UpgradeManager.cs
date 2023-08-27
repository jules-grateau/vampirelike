using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Settings;
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

        UpgradeSettings Settings;

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


        private UpgradeManager(string upgradeType)
        {
            _upgradeList = new List<UpgradeSO>();
            foreach (string upgrade in upgradeType.Split(";"))
            {
                _upgradeList.AddRange(Resources.LoadAll<UpgradeSO>($"ScriptableObjects/Upgrade/{upgrade}"));
            }
            Settings = Resources.Load<UpgradeSettings>("ScriptableObjects/Settings/UpgradesSetting");
        }

        public List<Upgrade<UpgradeSO>> GetAvailableUpgrades()
        {
            return FilterUpgrades(_upgradeList);
        }

        public Color GetUpgradeColor(UpgradeQuality quality)
        {
            return Settings.Color.GetColor(quality);
        }

        List<Upgrade<UpgradeSO>> FilterUpgrades(List<UpgradeSO> upgrades)
        {
            IEnumerable<UpgradeSO> filteredUpgrades = upgrades;
            WeaponInventoryManager weaponManager = GameManager.GameState.Player.GetComponent<WeaponInventoryManager>();
            PlayerStatsController playerStats = GameManager.GameState.Player.GetComponent<PlayerStatsController>();
            List<UpgradeSO> previousUpgrades = playerStats.Upgrades.Select((upgrade) => (UpgradeSO) upgrade.UpgradeSO)
                .Concat(weaponManager.Upgrades.Select((upgrade) => (UpgradeSO) upgrade.UpgradeSO)).ToList();
            List<Upgrade<UpgradeSO>> _availableUpgrades = new List<Upgrade<UpgradeSO>>();

            //Filter out MaxAmount upgrades
            Dictionary<UpgradeSO, int> previousUpgradesDictionnary =
                previousUpgrades.GroupBy(upgrade => upgrade)
                .ToDictionary(group => group.Key, group => group.Count());

            filteredUpgrades = filteredUpgrades.Where(upgrade => upgrade.MaxAmount == 0 || upgrade.MaxAmount > previousUpgradesDictionnary.GetValueOrDefault(upgrade));

            //Filter weapon specific upgrades
            List<WeaponSO> weapons = weaponManager.Inventory.Select(weaponController => weaponController.weapon).ToList();
            filteredUpgrades = filteredUpgrades.Where(upgrade => upgrade is not SpecificWeaponStatsUpgradeSO || weapons.Contains((upgrade as SpecificWeaponStatsUpgradeSO).ForWeapon));


            _availableUpgrades = filteredUpgrades.Where((upgradeSO) => upgradeSO is CharacterStatsUpgradeSO)
            .SelectMany((upgradeSO) =>
            {
                

                CharacterStatsUpgradeSO charactUpgrade = upgradeSO as CharacterStatsUpgradeSO;
                return charactUpgrade.GetDropableUpgrades(playerStats.CharacterStatistics);

            }).ToList();

            _availableUpgrades.AddRange(filteredUpgrades.Where((upgradeSO) => upgradeSO is WeaponStatsUpgradeSO)
                .SelectMany((upgradeSO) =>
                {
                    WeaponStatsUpgradeSO weaponUpgrade = upgradeSO as WeaponStatsUpgradeSO;
                    return weaponUpgrade.GetDropableUpgrades(weaponManager.WeaponStats);

                })
            );

            return _availableUpgrades;
        }

        public List<Upgrade<UpgradeSO>> Draw(int nbToDraw)
        {
            List <Upgrade<UpgradeSO>> upgrades = GetAvailableUpgrades();
            List<Upgrade<UpgradeSO>> upgradesToShow = new List<Upgrade<UpgradeSO>>();
            Dictionary<UpgradeQuality, List<Upgrade<UpgradeSO>>> upgradesByQuality = new Dictionary<UpgradeQuality, List<Upgrade<UpgradeSO>>>();

            foreach(UpgradeQuality quality in Enum.GetValues(typeof(UpgradeQuality)))
            {
                upgradesByQuality.Add(quality, upgrades.Where((upg) => upg.UpgradeQuality == quality).ToList());
            }


            while (upgradesToShow.Count < nbToDraw)
            {
                float randomQualityNumber = UnityEngine.Random.Range(0, 100) / 100f;
                UpgradeQuality[] qualityList = _qualityDropChance.Where(item => randomQualityNumber <= item.Value)
                    .OrderBy(item => item.Value)
                    .Select(item => item.Key).ToArray();

                int qualityIndex = 0;
                List<Upgrade<UpgradeSO>> qualityRelatedUpgrades = null;
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

                upgradesToShow.Add(qualityRelatedUpgrades[randomUpgradeIndex]);
                qualityRelatedUpgrades.RemoveAt(randomUpgradeIndex);
            }

            return upgradesToShow;
        }

        public int GetRedrawInitialCost()
        {
            return Settings.RedrawCost.InitialCost;
        }

        public int GetNextCost(int currCost)
        {
            return Settings.RedrawCost.GetNextCost(currCost);
        }


    }
}