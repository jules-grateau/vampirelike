using Assets.Scripts.Events;
using System.Collections;
using System;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;

namespace Assets.Scripts.Controller.Collectible
{
    [Serializable]
    public abstract class CollectibleItem : MonoBehaviour
    {
        public AudioClip pickupSound { get; set; }
        public GameEventCollectible OnCollectEvent { get; set; }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Collect(collision);
                Destroy(gameObject);
            }
        }

        protected void Collect(Collider2D collision)
        {
            if(pickupSound)
            {
                AudioSource.PlayClipAtPoint(pickupSound, this.gameObject.transform.position);
            }
            CustomCollectEvent(collision);
            OnCollectEvent.Raise(this);
        }

        protected virtual void CustomCollectEvent(Collider2D collision) {

        }
    }
}