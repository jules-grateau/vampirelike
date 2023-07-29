using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Events.TypedEvents;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DamagePerSecondeProjectile : ProjectileCollision
    {
        [SerializeField]
        public GameEventHitData enemyHitEvent;
        [SerializeField]
        public float tickSpeed;
        private float _cooloff;
        protected override void HandleEnemyCollision(Collision2D collision2D)
        {
            base.HandleEnemyCollision(collision2D);
            if (_cooloff >= tickSpeed)
            {
                enemyHitEvent.Raise(new HitData {
                    damage = damage,
                    instanceID = collision2D.gameObject.GetInstanceID(),
                    position = collision2D.transform.position,
                    source = parent
                });
                _cooloff = 0f;
            }
            _cooloff += Time.deltaTime;
        }
    }
}