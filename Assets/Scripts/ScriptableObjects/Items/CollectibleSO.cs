using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class CollectibleSO : ScriptableObject
    {
        [SerializeField]
        protected AudioClip pickupAudio;
        public abstract void Collect(Vector3 position);
    }
}