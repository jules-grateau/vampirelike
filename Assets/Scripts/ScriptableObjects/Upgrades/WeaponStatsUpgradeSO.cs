using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponStatsUpgrade", menuName = "Upgrade/WeaponStats", order = 1)]
    public class WeaponStatsUpgradeSO : BaseStatsUpgradeSO<WeaponStatisticEnum>
    {
    }
}