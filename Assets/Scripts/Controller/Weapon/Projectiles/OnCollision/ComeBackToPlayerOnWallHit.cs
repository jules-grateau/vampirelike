using Assets.Scripts.Controller.Game;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class ComeBackToPlayerOnWallHit : OnCollisionDamageBehaviour
    {

        [SerializeField]
        public int numberOfHits;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator<Collision2D> self, Collision2D collision2D)
        {
            if (collision2D.gameObject.tag == "Wall")
            {
                Vector2 direction = GameManager.GameState.Player.transform.position - self.transform.position;
                self.transform.right = direction.normalized;
            }
            return;
        }
    }
}