using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/MagnetPower", order = 1)]
    public class MagnetPowerCollectible : PowerCollectible
    {

        public override void CollectON(Vector3 position)
        {
            var hits = Physics2D.OverlapCircleAll(position, Mathf.Infinity, 1 << LayerMask.NameToLayer("Collectible"));

            if (hits.Length > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    hit.gameObject.GetComponent<GetRadiusPlayerController>().forceCollect = true;
                }
            }
        }

        public override void CollectOFF(Vector3 position)
        {
        }
    }
}