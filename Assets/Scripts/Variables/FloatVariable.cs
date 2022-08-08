using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Variables
{
    [CreateAssetMenu]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField]
        private float _value;
        public float value { get { return _value; } set { _value = value; } }
    }
}