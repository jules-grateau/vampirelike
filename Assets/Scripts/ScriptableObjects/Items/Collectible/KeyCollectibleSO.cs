using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;
using Assets.Scripts.Controller.Game;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Key", order = 1)]
    public class KeyCollectibleSO : CollectibleSO<KeyCollectible>
    {
        public override KeyCollectible GetCollectible(Vector3 position)
        {
            KeyCollectible keyCollectibleController = base.GetCollectible(position);
            keyCollectibleController.Interactible = KeyController.Instance.queuChestGen(keyCollectibleController);
            
            return keyCollectibleController;
        }
    }
}
