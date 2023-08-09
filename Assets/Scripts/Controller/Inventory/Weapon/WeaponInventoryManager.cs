using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using Assets.Scripts.Types;
using Assets.Scripts.Types.Upgrades;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller.Inventory.Weapons
{
    public class WeaponInventoryManager : MonoBehaviour
    {
        public List<WeaponController> Inventory => _inventory;
        private List<WeaponController> _inventory = new List<WeaponController>();
        public List<WeaponStatsUpgrade> Upgrades => _upgrades;
        private List<WeaponStatsUpgrade> _upgrades = new();
        public BaseStatistics<WeaponStatisticEnum> WeaponStats => _weaponStats;
        private BaseStatistics<WeaponStatisticEnum> _weaponStats;

        public void Init(BaseStatistics<WeaponStatisticEnum> weaponStatistics)
        {
            if (weaponStatistics == null) return;
            _weaponStats = weaponStatistics;
        }

        public void EquipWeapon(WeaponSO weapon)
        {
            Debug.Log($"Equipped weapon {weapon.name}");

            WeaponController wpController = gameObject.AddComponent<WeaponController>();
            weapon.Init(_weaponStats);
            wpController.weapon = weapon;
            _inventory.Add(wpController);
        }

        public void OnSelectUpgrade(Upgrade<UpgradeSO> upgrade)
        {
            if (upgrade.UpgradeSO is WeaponStatsUpgradeSO)
            {
                WeaponStatsUpgrade weaponUpgrade = new WeaponStatsUpgrade(upgrade.UpgradeQuality,(WeaponStatsUpgradeSO) upgrade.UpgradeSO);
                _upgrades.Add(weaponUpgrade);
                HandleStatUpgrade(weaponUpgrade, _inventory);
            }
        }

        void HandleStatUpgrade(WeaponStatsUpgrade upgrade, List<WeaponController> weapons)
        {
            WeaponSO forWeapon = upgrade.UpgradeSO is SpecificWeaponStatsUpgradeSO ? ((SpecificWeaponStatsUpgradeSO) upgrade.UpgradeSO)._forWeapon : null;

            if (!forWeapon)
            {
                _weaponStats.UpgradeStats(upgrade.UpgradeSO.StatsToUpgrade, upgrade.GetValue(), upgrade.UpgradeSO.AdditionType, upgrade.UpgradeSO.MaxValue);
                return;
            }

            foreach (WeaponController weapon in weapons)
            {
                if (!forWeapon || forWeapon.Equals(weapon.weapon))
                {
                    weapon.weapon.UpgradeStats(upgrade.UpgradeSO.StatsToUpgrade, upgrade.GetValue(), upgrade.UpgradeSO.AdditionType, upgrade.UpgradeSO.MaxValue);
                }
            }

        }
    }
}