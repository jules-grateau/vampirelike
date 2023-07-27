using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    public class BaseStatisticDescriptionSO<T> : ScriptableObject
    {
        public T Key => _key;
        [SerializeField]
        T _key;

        public string Name => _name;
        [SerializeField]
        string _name;

        public string ValueAppendix => _valueAppendix;
        [SerializeField]
        string _valueAppendix;
    }
}