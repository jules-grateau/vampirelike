using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class DamagePlayerOnCollision : MonoBehaviour
    {
        [SerializeField]
        float _damage;
        [SerializeField]
        GameEventFloat playerCollisionEvent;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                playerCollisionEvent.Raise(_damage);
            }
        }
    }
}