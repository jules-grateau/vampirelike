using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Collectible
{
    public abstract class PowerCollectible : CollectibleItem
    {
        public float Duration {get;set;}

        protected override void Collect()
        {
            base.Collect();
            StartCoroutine(TriggerCollect());
        }

        public abstract void CollectON();

        public abstract void CollectOFF();

        private IEnumerator TriggerCollect()
        {
            CollectON();
            yield return new WaitForSeconds(Duration);
            CollectOFF();
        }
    }
}