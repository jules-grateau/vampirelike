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

        public float damage { get ; set; }

        protected override void HandleEnemyCollision(Collider2D collider2D)
        {
            Debug.Log("Dealing damage");
            Debug.Log(_cooloff);
            Debug.Log(tickSpeed);
            Debug.Log(Time.deltaTime);
            if(_cooloff >= tickSpeed)
            {
                Debug.Log("Hit");
                enemyHitEvent.Raise(new HitData { damage = damage, instanceID = collider2D.gameObject.GetInstanceID(), position = collider2D.transform.position });
                _cooloff = 0f;
            }
            
            _cooloff += Time.deltaTime;
            Debug.Log(_cooloff);

        }
    }
}