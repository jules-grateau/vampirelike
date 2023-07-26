using Assets.Scripts.Variables;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StatsUpgrade", menuName = "Upgrade/Stats", order = 1)]
    public class StatsUpgradeSO : UpgradeSO
    {
        [SerializeField]
        FloatVariable _statsToUpgrade;
        [SerializeField]
        float _valueToAdd;


        public override void Consume()
        {
            _statsToUpgrade.value += _valueToAdd;
        }
    }
}