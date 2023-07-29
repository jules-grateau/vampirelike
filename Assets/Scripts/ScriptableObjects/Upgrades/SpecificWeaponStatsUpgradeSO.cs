using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Items;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponStatsUpgrade", menuName = "Upgrade/SpecificWeaponStats", order = 1)]
    public class SpecificWeaponStatsUpgradeSO : WeaponStatsUpgradeSO
    {
        public WeaponSO ForWeapon => _forWeapon;
        [SerializeField]
        public WeaponSO _forWeapon;
    }
}