using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Types
{
    [Serializable]
    public class Statistic
    {
        public StatisticEnum Key => _key;
        [SerializeField]
        StatisticEnum _key;

        public float Value => _value;
        [SerializeField]
        float _value;
    }
}