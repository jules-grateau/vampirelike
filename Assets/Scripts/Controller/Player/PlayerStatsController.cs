using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStatsController : MonoBehaviour
{
    public CharacterStatisticsSO CharacterStatistics => _characterStatistics;

    CharacterStatisticsSO _characterStatistics;

    public void Init(CharacterStatisticsSO characterStatistics) 
    {
        _characterStatistics = characterStatistics;
        characterStatistics.Init();
    }

    public (bool, float) ComputeDamage(float damage)
    {
        float rand = Random.value;
        bool isCrit = rand <= _characterStatistics.GetStats(StatisticEnum.CritChance)/ 100f;
        float computedDamage = damage;

        if (isCrit)
        {
            computedDamage = computedDamage * (_characterStatistics.GetStats(StatisticEnum.CritDamage) / 100f);
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
    }
}
