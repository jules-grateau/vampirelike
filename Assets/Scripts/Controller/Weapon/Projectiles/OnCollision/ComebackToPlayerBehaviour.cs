using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class ComebackToPlayerBehaviour : OnCollisionDamageBehaviour
    {

        [SerializeField]
        public int numberOfHits;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
        }

        public override void HandleBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
            if (((OnCollisionBehaviourOrchestrator)self).alreadyTargeted.Count >= numberOfHits)
            {
                self.transform.rotation = Quaternion.Slerp(self.transform.rotation, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (parent.transform.position - self.transform.position)), 0.8f);
                var hits = Physics2D.OverlapCircle(self.transform.position, 0.1f, 1 << LayerMask.NameToLayer("Player"));
                if (!hits) return;
                GameObject.Destroy(self.gameObject);
            }
        }
    }
}