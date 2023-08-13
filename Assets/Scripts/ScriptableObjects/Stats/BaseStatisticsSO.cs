using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    public abstract class BaseStatisticsSO<T> : ScriptableObject
    {
        [SerializeField]
        Statistic<T>[] _baseStats;

        public Dictionary<T, float> _stats;

        public virtual void Init(BaseStatisticsSO<T> additionalStats = null)
        {
            _stats = new Dictionary<T, float>();

            foreach(Statistic<T> val in _baseStats)
            {
                float additionalValue = 0;
                if(additionalStats)
                {
                    additionalValue = additionalStats.GetStats(val.Key);
                }
                _stats.Add(val.Key, val.Value + additionalValue);
            }

            foreach (T weaponStatsEnum in Enum.GetValues(typeof(T)))
            {
                if (_stats.ContainsKey(weaponStatsEnum)) continue;

                _stats.Add(weaponStatsEnum, 0);
            }
        }

        public float GetStats(T statisticEnum)
        {
            return _stats.GetValueOrDefault(statisticEnum);
        }

        public void UpgradeStats(T statisticEnum, float value, AdditionTypes additionType, float maxValue)
        {
            if (!_stats.ContainsKey(statisticEnum))
            {
                _stats.Add(statisticEnum, value);
                return;
            }
            
            switch(additionType)
            {
                case AdditionTypes.Additive:
                    _stats[statisticEnum] += value;
                    break;
                case AdditionTypes.Multiplicative:
                    float currValue = _stats[statisticEnum];
                    if (currValue == 0f)
                    {
                        _stats[statisticEnum] = value;
                    }
                    else
                    {
                        //Diminishing return by adding the percentage of what is left to be added
                        _stats[statisticEnum] += (maxValue - currValue) * (value / 100);
                    }
                    break;

            }

            OnUpgradeStats();
        }

        // Function to override in Childs if behavior wanted on Upgrade
        protected virtual void OnUpgradeStats()
        {
            return;
        }
    }
}