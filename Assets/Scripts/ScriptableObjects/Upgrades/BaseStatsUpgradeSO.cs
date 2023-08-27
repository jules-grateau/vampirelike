using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    public class BaseStatsUpgradeSO<T> : UpgradeSO
    {
        public T StatsToUpgrade => _statsToUpgrade;
        [SerializeField]
        public T _statsToUpgrade;

        public UpgradeQualityBasedValue<float>[] ValueToAdd => _valueToAdd;
        [SerializeField]
        public UpgradeQualityBasedValue<float>[]  _valueToAdd;

        public AdditionTypes AdditionType => _additionType;
        [SerializeField]
        public AdditionTypes _additionType;

        public float DropFrom => _dropFrom;
        public float DropUntil => _dropUntil;

        [Header("Drop condition")]
        [SerializeField]
        public DropCondition dropConditionTypes;

        [DrawIf("dropConditionTypes", DropCondition.Range, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        [Description("The stats value from which the upgrade will show")]
        float _dropFrom;
        [DrawIf("dropConditionTypes", DropCondition.Range, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        [Description("The stats value until which the upgrade will show")]
        float _dropUntil;
        [DrawIf("dropConditionTypes", DropCondition.OtherStats, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        T _otherStats;


        public float MaxValue => _maxValue;

        [DrawIf("_additionType", AdditionTypes.DiminishingReturn, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        float _maxValue;

        public override string GetDescription(UpgradeQuality upgradeQuality)
        {
            float val = _valueToAdd.FirstOrDefault((value) => value.Quality == upgradeQuality).Value;
            if (_additionType.Equals(AdditionTypes.Multiplicative))
            {
                val = Mathf.RoundToInt(( val - 1 ) * 100);
            }
            return _description.GetLocalizedString().Replace("{value}", val.ToString());
        }

        public float GetValue(UpgradeQuality upgradeQuality)
        {
            return _valueToAdd.FirstOrDefault((value) => value.Quality == upgradeQuality).Value;
        }

        bool IsDropable(UpgradeQuality upgradeQuality, BaseStatistics<T> currStats)
        {
            float currValue = currStats.GetStats(_statsToUpgrade);
            float upgradedValue = currValue + GetValue(upgradeQuality);

            switch(dropConditionTypes)
            {
                //TODO : Find a way to avoid reaching over range, without getting stuck when too close to max
                case DropCondition.Range:
                    return currValue >= _dropFrom && currValue < _dropUntil || _dropFrom == _dropUntil;
                case DropCondition.OtherStats:
                    return upgradedValue <= currStats.GetStats(_otherStats);
                default:
                    return true;

            }
        }


        public List<Upgrade<UpgradeSO>> GetDropableUpgrades(BaseStatistics<T> currStats)
        {
            List<Upgrade<UpgradeSO>> _dropableList = new List<Upgrade<UpgradeSO>>();

            foreach(UpgradeQualityBasedValue<float> upgradeQuality in _valueToAdd)
            {
                if(!IsDropable(upgradeQuality.Quality, currStats)) continue;

                _dropableList.Add(new Upgrade<UpgradeSO>(upgradeQuality.Quality, this));

            }
            return _dropableList;
        }

        public override bool HasQuality(UpgradeQuality quality)
        {
            return _valueToAdd.Any((value) => value.Quality == quality);
        }
    }
}