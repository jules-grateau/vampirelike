using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Events.TypedEvents;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DamagePerSecondeProjectile : ProjectileTrigger, IDamageProjectile
    {
        [SerializeField]
        private GameEventHitData enemyHitEvent;
        [SerializeField]
        private float tickSpeed;
        private float _cooloff;

        public GameObject parent { get; set; }
        public float damage { get ; set; }

        protected override void HandleEnemyCollision(Collider2D collider2D)
        {
            if(_cooloff >= tickSpeed)
            {
                enemyHitEvent.Raise(new HitData {
                    damage = damage,
                    instanceID = collider2D.gameObject.GetInstanceID(),
                    position = collider2D.transform.position,
                    source = parent
                });
                _cooloff = 0f;
            }
            
            _cooloff += Time.deltaTime;
        }
    }
}