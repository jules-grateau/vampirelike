using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DestroyOnEndBehaviour : ProgressBehaviour
    {

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Destroy };
        }

        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
            self.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.Destroy(self.gameObject);
        }
    }
}