using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Inventory.Weapons
{
    public class WeaponInventoryManager : MonoBehaviour
    {
        private List<WeaponController> _inventory = new List<WeaponController>();
        private List<WeaponStatsUpgradeSO> _upgrades = new List<WeaponStatsUpgradeSO>();

        public void Init(WeaponStatisticsSO weaponStatistics)
        {
            if (!weaponStatistics) return;
            weaponStatistics.Init();
            WeaponStatsUpgradeSO[] wUpgrades = Resources.LoadAll<WeaponStatsUpgradeSO>($"ScriptableObjects/Upgrade/Weapon");
            foreach (KeyValuePair<WeaponStatisticEnum, float> weaponStatistic in weaponStatistics._stats)
            {
                foreach (WeaponStatsUpgradeSO wUpgrade in wUpgrades)
                {
                    if (wUpgrade._statsToUpgrade.Equals(weaponStatistic.Key))
                    {
                        WeaponStatsUpgradeSO wUpgradeCopy = Instantiate(wUpgrade);
                        wUpgradeCopy._valueToAdd = weaponStatistic.Value;
                        _upgrades.Add(wUpgradeCopy);
                    }
                }
            }
                
        }

        public void EquipWeapon(WeaponSO weapon)
        {
            WeaponController wpController = gameObject.AddComponent<WeaponController>();
            wpController.weapon = weapon;
            foreach (WeaponStatsUpgradeSO upgrade in _upgrades)
            {
                HandleStatUpgrade(upgrade, new List<WeaponController>() { wpController });
            }
            _inventory.Add(wpController);
        }

        public void OnSelectUpgrade(UpgradeSO upgrade)
        {
            if (upgrade is WeaponStatsUpgradeSO)
            {
                _upgrades.Add((WeaponStatsUpgradeSO)upgrade);
                HandleStatUpgrade((WeaponStatsUpgradeSO)upgrade, _inventory);
            }
        }

        void HandleStatUpgrade(WeaponStatsUpgradeSO upgrade, List<WeaponController> weapons)
        {
            WeaponSO forWeapon = upgrade is SpecificWeaponStatsUpgradeSO ? ((SpecificWeaponStatsUpgradeSO)upgrade)._forWeapon : null;
            foreach (WeaponController weapon in weapons)
            {
                if (!forWeapon || forWeapon.Equals(weapon.weapon))
                {
                    weapon.weapon.UpgradeStats(upgrade.StatsToUpgrade, upgrade.ValueToAdd, upgrade.AdditionType, upgrade.MaxValue);
                }
            }
        }
    }
}