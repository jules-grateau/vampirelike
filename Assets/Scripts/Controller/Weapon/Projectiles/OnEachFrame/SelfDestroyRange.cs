using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRange : ProgressBehaviour
    {
        public float Range { get; set; }

        private Vector3 initPosition;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float time)
        {
            initPosition = self.transform.position;
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float time)
        {
            if (Vector2.Distance(initPosition, self.transform.position) > Range)
            {
                GameObject.Destroy(self.gameObject);
                return;
            }
        }
    }
}