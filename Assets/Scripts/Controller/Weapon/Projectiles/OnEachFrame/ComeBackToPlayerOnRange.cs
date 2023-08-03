using Assets.Scripts.Controller.Game;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame
{
    public class ComeBackToPlayerOnRange : ProgressBehaviour
    {
        public float Range;

        bool reachedRange = false;
        public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float payload)
        {
            if(reachedRange)
            {
                Vector2 direction = GameManager.GameState.Player.transform.position - self.transform.position;
                self.transform.right = direction.normalized;
                return;
            }

            float distance = Vector2.Distance(self.transform.position, GameManager.GameState.Player.transform.position); 
            if (distance < Range) return;
            reachedRange = true;

        }

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float payload)
        {
        }


    }
}