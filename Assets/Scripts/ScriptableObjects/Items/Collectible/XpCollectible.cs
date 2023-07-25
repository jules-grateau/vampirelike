using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Basic", order = 1)]
    public class XpCollectible : CollectibleSO
    {
        [SerializeField]
        public float _xpValue;
        public override void Collect(Vector3 position)
        {
            AudioSource.PlayClipAtPoint(pickupAudio, position, 1);
        }
    }
}