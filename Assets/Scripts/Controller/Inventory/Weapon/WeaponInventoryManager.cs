using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;

namespace Assets.Scripts.Controller.Inventory.Weapons
{
    public class WeaponInventoryManager : MonoBehaviour
    {
        public void EquipWeapon(WeaponSO weapon)
        {
            WeaponController wpController = gameObject.AddComponent<WeaponController>();
            wpController.weapon = weapon;
        }
    }
}