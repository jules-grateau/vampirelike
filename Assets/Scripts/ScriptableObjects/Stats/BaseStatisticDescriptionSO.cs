using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    public abstract class BaseStatisticDescriptionSO<T> : ScriptableObject
    {
        public T Key => _key;
        [SerializeField]
        T _key;

        public string Name => _name.GetLocalizedString();
        [SerializeField]
        LocalizedString _name;

        public string ValueAppendix => _valueAppendix;
        [SerializeField]
        string _valueAppendix;

        public float MaxValue => _maxValue;
        [SerializeField]
        float _maxValue;
    }
}