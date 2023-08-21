using UnityEditor;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Controller.Collectible;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class BaseCollectibleSO: ScriptableObject
    {
        [SerializeField]
        protected AudioClip pickupAudio;
        [SerializeField]
        protected GameObject _prefab;
        [SerializeField]
        protected GameEventCollectible _collectEvent;
        [Range(0.0F, 1.0F)]
        [SerializeField]
        public float dropChance;
        [SerializeField]
        public bool CanBePulled = true;

        public abstract GameObject GetGameObject(Vector3 position);
    }
}