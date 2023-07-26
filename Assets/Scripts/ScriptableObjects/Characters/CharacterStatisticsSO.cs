using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "CharacterStatistics", menuName = "CharacterStatistics", order = 1)]
    public class CharacterStatisticsSO : ScriptableObject
    {
        [SerializeField]
        Statistic[] _baseStats;

        Dictionary<StatisticEnum, float> _stats;

        public void Init()
        {
            _stats = new Dictionary<StatisticEnum, float>();

            foreach(Statistic stats in _baseStats)
            {
                _stats.Add(stats.Key, stats.Value);
            }
        }

        public float GetStats(StatisticEnum statisticEnum)
        {
            return _stats.GetValueOrDefault(statisticEnum);
        }

        public void UpgradeStats(StatisticEnum statisticEnum, float value)
        {
            if (!_stats.ContainsKey(statisticEnum))
            {
                _stats.Add(statisticEnum, value);
                return;
            }

            _stats[statisticEnum] += value;
        }
    }
}