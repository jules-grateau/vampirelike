using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Weapon", order = 1)]
    public class WeaponCollectibleSO : CollectibleSO<WeaponCollectible>
    {
        public override WeaponCollectible GetCollectible(Vector3 position)
        {
            WeaponCollectible weaponCollectibleController = base.GetCollectible(position);
            weaponCollectibleController.Weapon = WeaponSpawnedController.Instance.getRandomWeaponSO();

            return weaponCollectibleController;
        }
    }
}