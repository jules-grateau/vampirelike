using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Xp", order = 1)]
    public class XpCollectibleSO : CollectibleSO
    {
        [SerializeField]
        private GameEventFloat _xpEvent;
        [SerializeField]
        private float defaultXpValue;

        public GameObject GetCollectible(float xpValue)
        {
            GameObject xpInstance = Instantiate(_prefab);
            xpInstance.SetActive(false);
            XpCollectible xpCollectibleController = xpInstance.AddComponent<XpCollectible>();
            xpCollectibleController.XpValue = xpValue;
            xpCollectibleController.OnPlayerGainXp = _xpEvent;
            xpInstance.AddComponent<GetRadiusPlayerController>();
            AudioSource audio = xpInstance.AddComponent<AudioSource>();
            audio.clip = pickupAudio;

            return xpInstance;
        }

        public override GameObject GetCollectible()
        {
            return GetCollectible(defaultXpValue);
        }
    }
}