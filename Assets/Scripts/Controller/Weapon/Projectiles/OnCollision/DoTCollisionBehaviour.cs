using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DoTBehaviour : OnCollisionDamageBehaviour
    {
        [SerializeField]
        public float tickSpeed;
        private float _cooloff;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start, ProjectileState.End };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            if (_cooloff >= tickSpeed)
            {
                enemyHitEvent.Raise(new HitData
                {
                    damage = damage,
                    instanceID = collision2D.gameObject.GetInstanceID(),
                    position = collision2D.transform.position,
                    source = self.parent
                });
                _cooloff = 0f;
            }
            _cooloff += Time.deltaTime;
        }
    }
}