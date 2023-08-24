using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Xp", order = 1)]
    public class XpCollectibleSO : CollectibleSO<XpCollectible>
    {
        [SerializeField]
        private float defaultValue;

        public XpCollectible GetCollectible(Vector3 position, float healthValue)
        {
            XpCollectible valueCollectibleController = base.GetCollectible(position);
            valueCollectibleController.Value = healthValue;
            valueCollectibleController.Type = Types.ValueCollectibleTypes.Xp;

            return valueCollectibleController;
        }

        public GameObject GetGameObject(Vector3 position, float xpValue)
        {
            return GetCollectible(position, xpValue).gameObject;
        }

        public override XpCollectible GetCollectible(Vector3 position)
        {
            return GetCollectible(position, defaultValue);
        }
    }
}