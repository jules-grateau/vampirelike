using Assets.Scripts.Controller.Collectible;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/MagnetPower", order = 1)]
    public class MagnetPowerCollectibleSO : PowerCollectibleSO
    {
        public override GameObject GetCollectible()
        {
            GameObject magnetInstance = Instantiate(_prefab);
            magnetInstance.SetActive(false);
            MagnetPowerCollectible magnetPowerCollectible =  magnetInstance.AddComponent<MagnetPowerCollectible>();
            magnetPowerCollectible.Duration = _duration;
            magnetInstance.AddComponent<GetRadiusPlayerController>();
            return magnetInstance;
        }
    }

}