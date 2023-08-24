using Assets.Scripts.Types;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [Serializable]
    public class BaseStatistics<T>
    {
        [SerializeField]
        Statistic<T>[] _baseStats;

        public Dictionary<T, float> Stats => _stats;
        Dictionary<T, float> _stats;

        public BaseStatistics()
        {

        }

        public void Init(BaseStatistics<T> additionalStats = null)
        {
            _stats = new Dictionary<T, float>();

            foreach (T statsEnum in Enum.GetValues(typeof(T)))
            {
                _stats.Add(statsEnum, 0);
            }
            if (_baseStats != null)
            {
                foreach (Statistic<T> val in _baseStats)
                {
                    _stats[val.Key] += val.Value;
                }
            }

            if(additionalStats != null)
            {
                foreach (KeyValuePair<T, float> stat in additionalStats.Stats)
                {
                    _stats[stat.Key] += stat.Value;
                }
            }

        }

        public float GetStats(T statisticEnum)
        {
            if (_stats == null) return 0f;
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