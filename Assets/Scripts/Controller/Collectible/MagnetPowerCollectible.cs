using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controller.Collectible
{
    public class MagnetPowerCollectible : PowerCollectible
    {
        protected override void CollectON(GameObject parent)
        {
            var hits = Physics2D.OverlapCircleAll(gameObject.transform.position, Mathf.Infinity, 1 << LayerMask.NameToLayer("Collectible"));
            hits = hits.Where((hit) => hit.gameObject.CompareTag("Collectible")).ToArray();
            if (hits.Length > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    if (!hit.gameObject) return;

                    CollectibleItem collectibleItem = hit.gameObject.GetComponent<CollectibleItem>();
                    if (collectibleItem != null) {
                        collectibleItem.Attract(parent.transform);
                    }
                }
            }
        }

        protected override void CollectOFF(GameObject parent)
        {
        }
    }
}