using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "Statistics", menuName = "Statistics", order = 8)]
    public class StatisticSO : ScriptableObject
    {
        public StatisticEnum Key => _key;
        [SerializeField]
        StatisticEnum _key;

        public string Name => _name;
        [SerializeField]
        string _name;

        public string ValueAppendix => _valueAppendix;
        [SerializeField]
        string _valueAppendix;
    }
}