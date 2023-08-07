using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    public class BaseStatsUpgradeSO<T> : UpgradeSO
    {
        public T StatsToUpgrade => _statsToUpgrade;
        [SerializeField]
        public T _statsToUpgrade;

        public float ValueToAdd => _valueToAdd;
        [SerializeField]
        public float _valueToAdd;

        public AdditionTypes AdditionType => _additionType;
        [SerializeField]
        public AdditionTypes _additionType;

        public float DropFrom => _dropFrom;
        public float DropUntil => _dropUntil;
        [Header("Drop condition")]
        [SerializeField]
        [Description("The stats value from which the upgrade will show")]
        float _dropFrom;
        [SerializeField]
        [Description("The stats value until which the upgrade will show")]
        float _dropUntil;


        public float MaxValue => _maxValue;

        [DrawIf("_additionType", AdditionTypes.Multiplicative, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        float _maxValue;

        public override string GetDescription()
        {
            return _description.Replace("{value}", _valueToAdd.ToString());
        }

        public bool IsDropable(float currValue)
        {
            return currValue >= _dropFrom && currValue  < _dropUntil || _dropFrom == _dropUntil;
        }
    }
}