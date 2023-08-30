using Assets.Scripts.Controller.Game;
using Assets.Scripts.Types;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyOnPlayerReach : OnCollisionDamageBehaviour
    {

        [SerializeField]
        public int numberOfHits;
        public ProjectileState TriggeredProjectileState = ProjectileState.End;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            if (collision2D.gameObject.GetInstanceID() == self.parent.gameObject.GetInstanceID())
            {
                self.TriggerNewState(TriggeredProjectileState);
            }
            return;
        }
    }
}