using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class EndOnWallHit : OnCollisionDamageBehaviour
    {

        [SerializeField]
        public int numberOfHits;

        public ProjectileState TriggeredProjectileState = ProjectileState.End;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start, ProjectileState.End };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            if (collision2D.gameObject.tag == "Wall")
            {
                self.TriggerNewState(TriggeredProjectileState);
            }
            return;
        }
    }
}