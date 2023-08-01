using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRandomDelay : ProgressBehaviour
    {

        [SerializeField]
        public float minDelay;
        [SerializeField]
        public float maxDelay;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float time)
        {
            GameObject.Destroy(self.gameObject, Random.Range(minDelay, maxDelay));
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float time)
        {
        }
    }
}