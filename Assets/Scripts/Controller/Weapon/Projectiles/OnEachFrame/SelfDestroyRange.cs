using UnityEditor;
using UnityEngine;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRange : ProgressBehaviour
    {
        public float Range { get; set; }

        public ProjectileState TriggeredProjectileState = ProjectileState.End;

        public Vector3 InitPosition { get; set; }

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start };
            InitPosition = self.transform.position;
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
            if (Vector2.Distance(InitPosition, self.transform.position) > Range)
            {
                self.TriggerNewState(TriggeredProjectileState);
                return;
            }
        }
    }
}