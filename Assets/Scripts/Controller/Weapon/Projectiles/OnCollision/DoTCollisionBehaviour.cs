using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DoTBehaviour : OnCollisionDamageBehaviour
    {
        [SerializeField]
        public float tickSpeed;
        private float _cooloff;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
            if (_cooloff >= tickSpeed)
            {
                enemyHitEvent.Raise(new HitData
                {
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