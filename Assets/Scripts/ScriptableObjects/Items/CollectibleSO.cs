using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class CollectibleSO : ScriptableObject
    {
        public abstract void Collect();
    }
}