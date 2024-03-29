﻿using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class ExplodeBehaviour : DirectDamageBehaviour
    {
        [SerializeField]
        public float explosionRadius;
        [SerializeField]
        public ParticleSystem particles;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            base.HandleBehaviour(self, collision2D);
            ParticleSystem p = ParticleSystem.Instantiate(particles, self.transform.position, Quaternion.identity, self.transform);
            p.gameObject.transform.parent = null;
            p.Play();

            var hits = Physics2D.OverlapCircleAll(collision2D.transform.position, explosionRadius, 1 << LayerMask.NameToLayer("Enemy"));
            if (hits.Length <= 0) return;

            foreach (Collider2D hit in hits)
            {
                enemyHitEvent.Raise(new HitData
                {
                    status = status,
                    damage = damage,
                    instanceID = hit.gameObject.GetInstanceID(),
                    position = hit.transform.position,
                    source = self.parent,
                    weapon = self.parentWeapon
                });
            }
            return;
        }
    }
}