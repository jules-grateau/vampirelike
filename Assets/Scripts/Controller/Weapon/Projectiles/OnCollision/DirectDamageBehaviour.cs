using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DirectDamageBehaviour : OnCollisionDamageBehaviour
    {
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start, ProjectileState.End, ProjectileState.Destroy };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            if (collision2D.gameObject.GetInstanceID() == self.parent.gameObject.GetInstanceID()) return;
            if (collision2D.gameObject.tag != "Enemy" && collision2D.gameObject.tag != "Player") return;
            enemyHitEvent.Raise(new HitData
            {
                status = status,
                damage = damage,
                instanceID = collision2D.gameObject.GetInstanceID(),
                position = collision2D.transform.position,
                source = self.parent,
                weapon = self.parentWeapon
            });
        }
    }
}