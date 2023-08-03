using System.Collections;
using UnityEngine;
using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRandomDelay : ProgressBehaviour
    {

        [SerializeField]
        public float minDelay;
        [SerializeField]
        public float maxDelay;
        public float Duration { get; set; }

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start };
            self.StartCoroutine(ExecuteRandomTime(self));
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
        }
        IEnumerator ExecuteRandomTime(BaseBehaviourOrchestrator self)
        {
            float time = Random.Range(minDelay, maxDelay) * (1 + (Duration / 100));
            yield return new WaitForSeconds(time);
            self.TriggerNewState(Types.ProjectileState.End);
        }
    }
}