﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class ComebackToPlayerBehaviour : StraightMovementBehaviour
    {

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Destroy };
        }

        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
            base.HandleBehaviour(self, time);
            if(!self) return;

            self.gameObject.GetComponent<Rigidbody2D>().velocity = self.transform.right * speed;
            self.transform.rotation = Quaternion.Slerp(self.transform.rotation, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (self.parent.transform.position - self.transform.position)), 0.8f);
            
            var hits = Physics2D.OverlapCircle(self.transform.position, 0.1f, 1 << LayerMask.NameToLayer("Player"));
            if (!hits) return;
            GameObject.Destroy(self.gameObject);
        }
    }
}