using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Value", order = 1)]
    public class ValueCollectibleSO : CollectibleSO<ValueCollectible>
    {
        [SerializeField]
        private float _defaultValue;
        [SerializeField]
        private ValueCollectibleTypes _type;

        public ValueCollectible GetCollectible(Vector3 position, float value)
        {
            ValueCollectible valueCollectibleController = base.GetCollectible(position);
            valueCollectibleController.Value = value;
            valueCollectibleController.Type = _type;

            return valueCollectibleController;
        }

        public GameObject GetGameObject(Vector3 position, float xpValue)
        {
            return GetCollectible(position, xpValue).gameObject;
        }

        public override ValueCollectible GetCollectible(Vector3 position)
        {
            return GetCollectible(position, _defaultValue);
        }
    }
}