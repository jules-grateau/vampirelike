using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Types.Upgrades
{
    public class CharacterStatsUpgrade : StatsUpgrade<CharacterStatsUpgradeSO, CharacterStatisticEnum>
    {
        public CharacterStatsUpgrade(UpgradeQuality quality, CharacterStatsUpgradeSO upgradeSO) : base(quality, upgradeSO)
        {
        }
    }
}