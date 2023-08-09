using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Types.Upgrades
{
    public class WeaponStatsUpgrade : StatsUpgrade<WeaponStatsUpgradeSO, WeaponStatisticEnum>
    {
        public WeaponStatsUpgrade(UpgradeQuality quality, WeaponStatsUpgradeSO upgradeSO) : base(quality, upgradeSO)
        {
        }
    }
}