using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStatsController : MonoBehaviour
{
    public BaseStatistics<CharacterStatisticEnum> CharacterStatistics => _characterStatistics;
    BaseStatistics<CharacterStatisticEnum> _characterStatistics;
    public List<CharacterStatsUpgradeSO> Upgrades => _upgrades;
    List<CharacterStatsUpgradeSO> _upgrades = new List<CharacterStatsUpgradeSO>();

    public void Init(BaseStatistics<CharacterStatisticEnum> characterStatistics) 
    {
        _characterStatistics = characterStatistics;
    }

    public (bool, float) ComputeDamage(float damage)
    {
        float rand = Random.value;
        bool isCrit = rand <= _characterStatistics.GetStats(CharacterStatisticEnum.CritChance)/ 100f;
        float computedDamage = damage;

        if (isCrit)
        {
            computedDamage = computedDamage * (_characterStatistics.GetStats(CharacterStatisticEnum.CritDamage) / 100f);
        }

        computedDamage = Mathf.FloorToInt(computedDamage);

        return (isCrit, computedDamage);
    }

    public void OnSelectUpgrade(UpgradeSO upgrade)
    {
        if(upgrade is CharacterStatsUpgradeSO)
        {
            HandleStatUpgrade((CharacterStatsUpgradeSO)upgrade);
        }
    }

    void HandleStatUpgrade(CharacterStatsUpgradeSO upgrade)
    {
        _characterStatistics.UpgradeStats(upgrade.StatsToUpgrade, upgrade.ValueToAdd, upgrade.AdditionType, upgrade.MaxValue);
        _upgrades.Add(upgrade);

    }
}
