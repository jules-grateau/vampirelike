using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Xp", order = 1)]
    public class XpCollectibleSO : CollectibleSO<XpCollectible>
    {
        [SerializeField]
        private float defaultXpValue;

        public XpCollectible GetCollectible(Vector3 position, float xpValue)
        {
            XpCollectible xpCollectibleController = base.GetCollectible(position);
            xpCollectibleController.XpValue = xpValue;

            return xpCollectibleController;
        }

        public GameObject GetGameObject(Vector3 position, float xpValue)
        {
            return GetCollectible(position, xpValue).gameObject;
        }

        public override XpCollectible GetCollectible(Vector3 position)
        {
            return GetCollectible(defaultXpValue);
        }
    }
}