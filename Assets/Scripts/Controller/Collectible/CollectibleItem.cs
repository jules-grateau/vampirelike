using Assets.Scripts.Events;
using System.Collections;
using System;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Controller.Props;

namespace Assets.Scripts.Controller.Collectible
{
    [Serializable]
    public abstract class CollectibleItem : Attractable
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
            else if (collision.gameObject.tag == "Enemy")
            {
                SeekCollectibleController seekCollectible = collision.gameObject.GetComponent<SeekCollectibleController>();
                if (!seekCollectible) return;
                seekCollectible.PickUp(this.gameObject);
            }
        }

        protected void Collect(Collider2D collision)
        {
            if(pickupSound)
            {
                AudioSource.PlayClipAtPoint(pickupSound, this.gameObject.transform.position);
            }
            OnCollectEvent.Raise(this);
        }
    }
}