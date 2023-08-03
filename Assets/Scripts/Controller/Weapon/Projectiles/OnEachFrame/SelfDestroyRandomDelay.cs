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

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringState = ProjectileState.Start;
            self.StartCoroutine(ExecuteRandomTime(self));
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
        {
        }
        IEnumerator ExecuteRandomTime(BaseBehaviourOrchestrator self)
        {
            float time = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(time);
            self.TriggerNewState(Types.ProjectileState.End);
        }
    }
}