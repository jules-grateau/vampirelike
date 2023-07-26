using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Variables
{
    [CreateAssetMenu]
    public class IntVariable : ScriptableObject
    {
        [SerializeField]
        private int _value;
        public int value { get { return _value; } set { _value = value; } }
    }
}