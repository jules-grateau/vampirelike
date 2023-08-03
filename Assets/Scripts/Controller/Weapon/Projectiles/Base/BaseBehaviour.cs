using System.Collections;
using UnityEngine;
using System.Collections.Generic;

using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class BaseBehaviour<T>
    {
        public ProjectileState triggeringState;
        public abstract void HandleBehaviour(BaseBehaviourOrchestrator orchestrator, T payload);
        public abstract void HandleStartBehaviour(BaseBehaviourOrchestrator orchestrator);
    }
}