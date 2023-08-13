using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using Assets.Scripts.Types.Upgrades;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStatsController : MonoBehaviour
{
    public BaseStatistics<CharacterStatisticEnum> CharacterStatistics => _characterStatistics;
    BaseStatistics<CharacterStatisticEnum> _characterStatistics;
    public List<CharacterStatsUpgrade> Upgrades => _upgrades;
    List<CharacterStatsUpgrade> _upgrades = new List<CharacterStatsUpgrade>();

    public void Init(BaseStatistics<CharacterStatisticEnum> characterStatistics) 
    {
        _characterStatistics = characterStatistics;
    }

    public (bool, float) ComputeDamage(float damage, bool cannotBeCrit = false)
    {
        float rand = Random.value;
        bool isCrit = !cannotBeCrit && rand <= _characterStatistics.GetStats(CharacterStatisticEnum.CritChance)/ 100f;
        float computedDamage = damage;

        if (isCrit)
        {
            computedDamage = computedDamage * (_characterStatistics.GetStats(CharacterStatisticEnum.CritDamage) / 100f);
        }

        computedDamage = Mathf.FloorToInt(computedDamage);

        return (isCrit, computedDamage);
    }

    public void OnSelectUpgrade(Upgrade<UpgradeSO> upgrade)
    {
        if(upgrade.UpgradeSO is CharacterStatsUpgradeSO)
        {
            HandleStatUpgrade( new CharacterStatsUpgrade(upgrade.UpgradeQuality, (CharacterStatsUpgradeSO) upgrade.UpgradeSO));
        }
    }

    void HandleStatUpgrade(CharacterStatsUpgrade upgrade)
    {
        _characterStatistics.UpgradeStats(upgrade.UpgradeSO.StatsToUpgrade, upgrade.GetValue(), upgrade.UpgradeSO.AdditionType, upgrade.UpgradeSO.MaxValue);
        _upgrades.Add(upgrade);

    }
}
