using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class AnimateOnHitBehaviour : OnCollisionDamageBehaviour
    {
        private bool _isDestroyed = false;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start, ProjectileState.End };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            Animator animator = self.gameObject.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isHit");
            }
            return;
        }
    }
}