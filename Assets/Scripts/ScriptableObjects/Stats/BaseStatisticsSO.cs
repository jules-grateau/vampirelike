using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    public abstract class BaseStatisticsSO<T> : ScriptableObject
    {
        [SerializeField]
        Statistic<T>[] _baseStats;

        Dictionary<T, float> _stats;

        public void Init()
        {
            _stats = new Dictionary<T, float>();

            // Init all filled weapon stats and 0 if not used
            foreach (T weaponStatsEnum in Enum.GetValues(typeof(T)))
            {
                float value = 0f;
                for (int i = 0; i < _baseStats.Length; i++)
                {
                    if (_baseStats[i].Key.Equals(weaponStatsEnum))
                    {
                        value = _baseStats[i].Value;
                    }
                }
                _stats.Add(weaponStatsEnum, value);
            }
        }

        public float GetStats(T statisticEnum)
        {
            return _stats.GetValueOrDefault(statisticEnum);
        }

        public void UpgradeStats(T statisticEnum, float value)
        {
            if (!_stats.ContainsKey(statisticEnum))
            {
                _stats.Add(statisticEnum, value);
                return;
            }

            _stats[statisticEnum] += value;
            OnUpgradeStats();
        }

        // Function to override in Childs if behavior wanted on Upgrade
        protected virtual void OnUpgradeStats()
        {
            return;
        }
    }
}