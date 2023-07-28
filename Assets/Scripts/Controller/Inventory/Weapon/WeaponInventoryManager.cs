using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;

namespace Assets.Scripts.Controller.Inventory.Weapons
{
    public class WeaponInventoryManager : MonoBehaviour
    {
        private List<WeaponController> _inventory = new List<WeaponController>();

        public void EquipWeapon(WeaponSO weapon)
        {
            WeaponController wpController = gameObject.AddComponent<WeaponController>();
            wpController.weapon = weapon;
            _inventory.Add(wpController);
        }

        public void OnSelectUpgrade(UpgradeSO upgrade)
        {
            if (upgrade is WeaponStatsUpgradeSO)
            {
                HandleStatUpgrade((WeaponStatsUpgradeSO)upgrade);
            }

        }

        void HandleStatUpgrade(WeaponStatsUpgradeSO upgrade)
        {
            foreach(WeaponController weapon in _inventory)
            {
                weapon.weapon.UpgradeStats(upgrade.StatsToUpgrade, upgrade.ValueToAdd, upgrade.AdditionType,upgrade.MaxValue);
            }
        }
    }
}