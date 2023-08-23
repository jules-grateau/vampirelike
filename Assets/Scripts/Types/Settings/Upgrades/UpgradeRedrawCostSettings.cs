using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Types.Settings.Upgrades
{
    [Serializable]
    public class UpgradeRedrawCostSettings
    {
        public int InitialCost => _initialCost;

        [SerializeField]
        int _initialCost;
        [SerializeField]
        int _step;
        [SerializeField]
        AdditionTypes _additionTypes;

        public int GetNextCost(int currCost)
        {
            int nextCost = currCost;
            switch(_additionTypes)
            {
                case AdditionTypes.Additive:
                    nextCost += _step;
                    break;
                case AdditionTypes.Multiplicative:
                    nextCost *= _step;
                    break;
                default:
                    break;
            }
            return nextCost;
        }
    }
}