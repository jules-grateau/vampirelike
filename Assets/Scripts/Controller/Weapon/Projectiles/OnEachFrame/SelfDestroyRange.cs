using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRange : ProgressBehaviour
    {
        public float Range { get; set; }
        [SerializeField]
        private float _range;

        private Vector3 initPosition;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float time)
        {
            initPosition = self.transform.position;
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float time)
        {
            if (Vector2.Distance(initPosition, self.transform.position) > _range)
            {
                GameObject.Destroy(self);
                return;
            }
        }
    }
}