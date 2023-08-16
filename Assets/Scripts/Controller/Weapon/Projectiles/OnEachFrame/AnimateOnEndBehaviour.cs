using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class AnimateOnHitBehaviour : MovementBehaviour
    {

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.End };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
            Animator animator = self.gameObject.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetBool("isDestroyed", true);
            }
            return;
        }
    }
}