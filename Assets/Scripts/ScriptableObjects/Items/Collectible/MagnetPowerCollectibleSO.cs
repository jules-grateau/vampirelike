using Assets.Scripts.Controller.Collectible;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/MagnetPower", order = 1)]
    public class MagnetPowerCollectibleSO : PowerCollectibleSO<MagnetPowerCollectible>
    {
        public override MagnetPowerCollectible GetCollectible(Vector3 position)
        {
            MagnetPowerCollectible magnetCollectibleController = base.GetCollectible(position);
            return magnetCollectibleController;
        }
    }

}