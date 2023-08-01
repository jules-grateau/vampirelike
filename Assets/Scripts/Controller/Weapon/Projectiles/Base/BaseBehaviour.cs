using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class BaseBehaviour<T>
    {
        public abstract void HandleBehaviour(BaseBehaviourOrchestrator<T> orchestrator, T payload);

        public abstract void HandleStartBehaviour(BaseBehaviourOrchestrator<T> orchestrator, T payload);
    }
}