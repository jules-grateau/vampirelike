﻿using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DoTBehaviour : OnCollisionDamageBehaviour
    {
        [SerializeField]
        public float duration;
        private float _cooloff;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start, ProjectileState.End };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            enemyHitEvent.Raise(new HitData
            {
                damage = damage,
                instanceID = collision2D.gameObject.GetInstanceID(),
                position = collision2D.transform.position,
                source = self.parent,
                time = duration,
                status = HitStatusEnum.Poison
            });
        }
    }
}