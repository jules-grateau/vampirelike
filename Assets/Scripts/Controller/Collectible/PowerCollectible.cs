using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Collectible
{
    public abstract class PowerCollectible : CollectibleItem
    {
        public float Duration {get;set;}

        public void TriggerEffect(GameObject parent)
        {
            StartCoroutine(TriggerCollect(parent));
        }

        protected abstract void CollectON(GameObject parent);

        protected abstract void CollectOFF(GameObject parent);

        private IEnumerator TriggerCollect(GameObject parent)
        {
            CollectON(parent);
            yield return new WaitForSeconds(Duration);
            CollectOFF(parent);
        }
    }
}