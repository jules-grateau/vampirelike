using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class NoAnimateOnEndBehaviour : MovementBehaviour
    {

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.End };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
            self.TriggerNewState(Types.ProjectileState.Destroy);
            return;
        }
    }
}