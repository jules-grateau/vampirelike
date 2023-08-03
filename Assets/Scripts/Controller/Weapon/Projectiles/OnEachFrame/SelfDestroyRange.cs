using UnityEditor;
using UnityEngine;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRange : ProgressBehaviour
    {
        public float Range { get; set; }
        [SerializeField]
        private float _range;

        private Vector3 initPosition;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringState = ProjectileState.Start;
            initPosition = self.transform.position;
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
            if (Vector2.Distance(initPosition, self.transform.position) > _range)
            {
                self.TriggerNewState(Types.ProjectileState.End);
                return;
            }
        }
    }
}