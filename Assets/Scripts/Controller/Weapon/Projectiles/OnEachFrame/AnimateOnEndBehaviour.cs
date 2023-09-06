using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class AnimateOnEndBehaviour : MovementBehaviour
    {
        private bool _isDestroyed = false;
        Animator _animator;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.End };
            _animator = self.gameObject.GetComponentInChildren<Animator>();
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
            if (_animator != null && !_isDestroyed)
            {
                _isDestroyed = true;
                _animator.SetBool("isDestroyed", _isDestroyed);
            } 
            
            //Destroy the projectile when Destroy animation is over
            if(_isDestroyed && _animator.GetCurrentAnimatorStateInfo(0).IsName("Destroy") 
                && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
            {
                self.TriggerNewState(ProjectileState.Destroy);
            }

            return;
        }
    }
}