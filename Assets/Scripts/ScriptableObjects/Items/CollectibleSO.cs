using UnityEditor;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Controller.Collectible;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class CollectibleSO<T> : BaseCollectibleSO where T : CollectibleItem
    {
        public virtual T GetCollectible(Vector3 position)
        {
            GameObject collectibleInstance = Instantiate(_prefab, position, Quaternion.identity);
            T collectibleController = collectibleInstance.AddComponent<T>();
            collectibleController.OnCollectEvent = _collectEvent;
            collectibleController.pickupSound = pickupAudio;
            GetRadiusPlayerController radiusController = collectibleInstance.AddComponent<GetRadiusPlayerController>();
            radiusController.CanBePulled = CanBePulled;

            return collectibleController;
        }

        public override GameObject GetGameObject(Vector3 position)
        {
            return GetCollectible(position).gameObject;
        }
    }
}