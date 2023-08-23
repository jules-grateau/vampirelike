using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Health", order = 1)]
    public class ValueCollectibleSO : CollectibleSO<ValueCollectible>
    {
        [SerializeField]
        private float defaultValue;

        public ValueCollectible GetCollectible(Vector3 position, float healthValue)
        {
            ValueCollectible valueCollectibleController = base.GetCollectible(position);
            valueCollectibleController.Value = healthValue;

            return valueCollectibleController;
        }

        public GameObject GetGameObject(Vector3 position, float xpValue)
        {
            return GetCollectible(position, xpValue).gameObject;
        }

        public override ValueCollectible GetCollectible(Vector3 position)
        {
            return GetCollectible(position, defaultValue);
        }
    }
}