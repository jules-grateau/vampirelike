using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DirectDamageBehaviour : OnCollisionDamageBehaviour
    {
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
            enemyHitEvent.Raise(new HitData
            {
                damage = damage,
                instanceID = collision2D.gameObject.GetInstanceID(),
                position = collision2D.transform.position,
                source = parent
            });
            return;
        }
    }
}