using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StatsUpgrade", menuName = "Upgrade/Stats", order = 1)]
    public class StatsUpgradeSO : UpgradeSO
    {
        public StatisticEnum StatsToUpgrade => _statsToUpgrade;
        [SerializeField]
        StatisticEnum _statsToUpgrade;

        public float ValueToAdd => _valueToAdd;
        [SerializeField]
        float _valueToAdd;
    }
}