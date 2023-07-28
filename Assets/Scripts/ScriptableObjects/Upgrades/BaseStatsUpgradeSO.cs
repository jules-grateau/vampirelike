using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    public class BaseStatsUpgradeSO<T> : UpgradeSO
    {
        public T StatsToUpgrade => _statsToUpgrade;
        [SerializeField]
        T _statsToUpgrade;

        public float ValueToAdd => _valueToAdd;
        [SerializeField]
        float _valueToAdd;

        public AdditionTypes AdditionType => _additionType;
        [SerializeField]
        public AdditionTypes _additionType;


        public float MaxValue => _maxValue;

        [DrawIf("_additionType", AdditionTypes.Multiplicative, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        float _maxValue;

        public override string getDescription()
        {
            return _description.Replace("{value}", _valueToAdd.ToString());
        }
    }
}