using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Collectible
{
    public abstract class PowerCollectible : CollectibleItem
    {
        public float Duration {get;set;}

        public void TriggerEffect(Collider2D collision)
        {
            StartCoroutine(TriggerCollect(collision));
        }

        public abstract void CollectON(Collider2D collider);

        public abstract void CollectOFF(Collider2D collider);

        private IEnumerator TriggerCollect(Collider2D collision)
        {
            CollectON(collision);
            yield return new WaitForSeconds(Duration);
            CollectOFF(collision);
        }
    }
}