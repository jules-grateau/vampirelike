using Assets.Scripts.Controller.Game;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame
{
    public class TurnTowardPlayerOnRange : StraightMovementBehaviour
    {
        public float Range;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.End };
        }

        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float payload)
        {
            base.HandleBehaviour(self, payload);
            float distance = Vector2.Distance(self.transform.position, GameManager.GameState.Player.transform.position); 
            if (distance < Range) return;
            Vector2 direction = GameManager.GameState.Player.transform.position - self.transform.position;
            self.transform.right = direction.normalized;
        }

    }
}