using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyNbrOfHits : OnCollisionDamageBehaviour
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
                GameObject.Destroy(self.gameObject);
            }
            return;
        }
    }
}