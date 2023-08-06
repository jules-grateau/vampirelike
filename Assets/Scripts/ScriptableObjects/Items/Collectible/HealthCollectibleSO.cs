using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Health", order = 1)]
    public class HealthCollectibleSO : CollectibleSO<HealthCollectible>
    {
        [SerializeField]
        private float defaultHealthValue;

        public HealthCollectible GetCollectible(Vector3 position, float healthValue)
        {
            HealthCollectible healthCollectibleController = base.GetCollectible(position);
            healthCollectibleController.HealthValue = healthValue;

            return healthCollectibleController;
        }

        public GameObject GetGameObject(Vector3 position, float xpValue)
        {
            return GetCollectible(position, xpValue).gameObject;
        }

        public override HealthCollectible GetCollectible(Vector3 position)
        {
            return GetCollectible(position, defaultHealthValue);
        }
    }
}