using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Events.TypedEvents;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DirectDamageProjectile : ProjectileCollision 
    {
        [SerializeField]
        public GameEventHitData enemyHitEvent;
        protected override void HandleEnemyCollision(Collision2D collision2D)
        {
            enemyHitEvent.Raise(new HitData {
                damage = damage,
                instanceID = collision2D.gameObject.GetInstanceID(),
                position = collision2D.transform.position,
                source = parent
            });
            if(destroyOnHit) Destroy(gameObject);
            return;
        }
    }
}