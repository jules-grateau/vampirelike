using Assets.Scripts.Controller.Enemies.Interface;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class DamagePlayerOnCollision : IEnemyDamage
    {
        [SerializeField]
        public GameEventFloat PlayerColisionEvent { get; set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                PlayerColisionEvent.Raise(Damage);
            }
        }
    }
}