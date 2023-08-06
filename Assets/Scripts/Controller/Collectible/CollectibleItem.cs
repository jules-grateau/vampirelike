using Assets.Scripts.Events;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;

namespace Assets.Scripts.Controller.Collectible
{
    public class CollectibleItem : MonoBehaviour
    {
        public AudioClip pickupSound { get; set; }
        public GameEventFloat OnCollectEvent { get; set; }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Collect();
                Destroy(gameObject);
            }
        }

        protected virtual void Collect()
        {
            if(pickupSound)
            {
                AudioSource.PlayClipAtPoint(pickupSound, this.gameObject.transform.position);
            }
        }
    }
}