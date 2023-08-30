using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyNbrOfHits : OnCollisionDamageBehaviour
    {

        [SerializeField]
        public int numberOfHits;
        public ProjectileState TriggeredProjectileState = ProjectileState.End;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            if (((OnAllBehaviourOrchestrator)self).alreadyTargeted.Count >= numberOfHits)
            {
                self.TriggerNewState(TriggeredProjectileState);
            }
            return;
        }
    }
}