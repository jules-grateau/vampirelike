using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Events.TypedEvents;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DirectDamageProjectile : ProjectileCollision, IDamageProjectile 
    {
        [SerializeField]
        private GameEventHitData enemyHitEvent;
        [SerializeField]
        private Boolean _destroyOnHit = true;

        public float damage { get; set; }

        protected override void HandleEnemyCollision(Collision2D collision2D)
        {
            enemyHitEvent.Raise(new HitData { damage = damage,instanceID = collision2D.gameObject.GetInstanceID(),position = collision2D.transform.position });
            if(_destroyOnHit) Destroy(gameObject);
            return;
        }
    }
}