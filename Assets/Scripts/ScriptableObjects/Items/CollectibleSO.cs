using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class CollectibleSO : ScriptableObject
    {
        [SerializeField]
        protected AudioClip pickupAudio;
        [SerializeField]
        protected GameObject _prefab;

        public abstract GameObject GetCollectible(Vector3 position);
    }
}