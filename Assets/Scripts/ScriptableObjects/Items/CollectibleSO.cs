using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class CollectibleSO : ScriptableObject
    {
        [SerializeField]
        public float _xpValue;
        public abstract void Collect();
    }
}