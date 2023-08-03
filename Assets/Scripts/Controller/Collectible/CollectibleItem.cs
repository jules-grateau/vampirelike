using Assets.Scripts.Events;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Collectible
{
    public class CollectibleItem : MonoBehaviour
    {
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
            AudioSource audio = GetComponent<AudioSource>();
            if(audio)
            {
                AudioSource.PlayClipAtPoint(audio.clip, this.gameObject.transform.position);
            }
        }
    }
}