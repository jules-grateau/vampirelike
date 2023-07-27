using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Types
{
    [Serializable]
    public class Statistic<T>
    {
        public T Key => _key;
        [SerializeField]
        T _key;

        public float Value => _value;
        [SerializeField]
        float _value;
    }
}