using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Events.TypedEvents;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class ExplodeDamageProjectile : ProjectileCollision 
    {
        [SerializeField]
        public GameEventHitData enemyHitEvent;
        [SerializeField]
        public float explosionRadius;
        [SerializeField]
        public ParticleSystem particles;

        protected override void HandleEnemyCollision(Collision2D collision2D)
        {
            base.HandleEnemyCollision(collision2D);

            ParticleSystem p = Instantiate(particles, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            p.gameObject.transform.parent = null;
            p.Play();

            var hits = Physics2D.OverlapCircleAll(collision2D.transform.position, explosionRadius, 1 << LayerMask.NameToLayer("Enemy"));
            if (hits.Length <= 0) return;

            foreach(Collider2D hit in hits)
            {
                enemyHitEvent.Raise(new HitData
                {
                    damage = damage,
                    instanceID = hit.gameObject.GetInstanceID(),
                    position = hit.transform.position,
                    source = parent
                });
            }
            return;
        }
    }
}