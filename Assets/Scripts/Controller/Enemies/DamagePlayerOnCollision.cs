using Assets.Scripts.Controller.Enemies.Interface;
using Assets.Scripts.Events.TypedEvents;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    public class DamagePlayerOnCollision : IEnemyDamage
    {
        [SerializeField]
        public GameEventHitData PlayerColisionEvent { get; set; }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if(collision2D.gameObject.CompareTag("Player"))
            {
                PlayerColisionEvent.Raise(new HitData
                {
                    status = Status,
                    damage = Damage,
                    instanceID = collision2D.gameObject.GetInstanceID(),
                    position = collision2D.transform.position,
                    source = gameObject
                });
            }
        }
    }
}