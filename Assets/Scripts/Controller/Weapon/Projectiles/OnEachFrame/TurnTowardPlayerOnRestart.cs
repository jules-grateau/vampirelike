using Assets.Scripts.Controller.Game;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Types;
using System.Linq;

namespace Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame
{
    public class TurnTowardPlayerOnRestart : MovementBehaviour
    {
        public float Range;
        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Restart };
        }

        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float payload)
        {
            if (!GameManager.GameState.Player) return;

            Vector2 direction = GameManager.GameState.Player.transform.position - self.transform.position;

            float distance = Vector2.Distance(GameManager.GameState.Player.transform.position, self.transform.position);
            var hit = Physics2D.Raycast(self.transform.position, direction.normalized, distance, 1 << LayerMask.NameToLayer("Wall"));

            if(hit.collider != null) {
                self.TriggerNewState(ProjectileState.End);
                return;
            }

            self.transform.right = direction.normalized;
            SelfDestroyRange selfDestroyRange = self.getEachFrameBehaviourByType<SelfDestroyRange>();
            selfDestroyRange.InitPosition = GameManager.GameState.Player.transform.position;
            self.TriggerNewState(ProjectileState.Start);
        }

    }
}