using UnityEngine;

namespace Assets.Scripts.Controller.Collectible
{
    public class MagnetPowerCollectible : PowerCollectible
    {
        public override void CollectON()
        {
            var hits = Physics2D.OverlapCircleAll(gameObject.transform.position, Mathf.Infinity, 1 << LayerMask.NameToLayer("Collectible"));

            if (hits.Length > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    GetRadiusPlayerController radiusPlayerController = hit.gameObject.GetComponent<GetRadiusPlayerController>();
                    if (radiusPlayerController != null) {
                        radiusPlayerController.forceCollect = true;
                    }
                }
            }
        }

        public override void CollectOFF()
        {
        }
    }
}