using Assets.Scripts.Controller.Game;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyOnPlayerReach : OnCollisionDamageBehaviour
    {

        [SerializeField]
        public int numberOfHits;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            if (collision2D.gameObject.GetInstanceID() == self.parent.gameObject.GetInstanceID())
            {
                GameObject.Destroy(self.gameObject);
            }
            return;
        }
    }
}