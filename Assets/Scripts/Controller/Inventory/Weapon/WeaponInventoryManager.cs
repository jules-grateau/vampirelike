using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using Assets.Scripts.Types;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller.Inventory.Weapons
{
    public class WeaponInventoryManager : MonoBehaviour
    {
        public List<WeaponController> Inventory => _inventory;
        private List<WeaponController> _inventory = new List<WeaponController>();
        public List<WeaponStatsUpgradeSO> Upgrades => _upgrades;
        private List<WeaponStatsUpgradeSO> _upgrades = new List<WeaponStatsUpgradeSO>();
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

            if (!forWeapon)
            {
                _weaponStats.UpgradeStats(upgrade.StatsToUpgrade, upgrade.ValueToAdd, upgrade.AdditionType, upgrade.MaxValue);
            }
        }
    }
}