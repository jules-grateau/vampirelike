using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Variables.Constants
{
    public class FloatReference
    {
        [SerializeField]
        public bool _useConstant = true;
        [SerializeField]
        public float _constantValue;
        [SerializeField]
        public FloatVariable _variable;

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            _useConstant = true;
            _constantValue = value;
        }

        public float value
        {
            get { return _useConstant ? _constantValue : _variable.value; }
        }


        public static implicit operator float(FloatReference reference)
        {
            return reference.value;
        }
    }
}