using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.Types;
using Assets.Scripts.ScriptableObjects.Items.Weapons;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SplitBehaviour : DirectDamageBehaviour
    {
        [SerializeField]
        public int splitNbr;
        [SerializeField]
        public int splitTimes;

        public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
        {
            triggeringStates = new ProjectileState[] { ProjectileState.Start };
        }
        public override void HandleBehaviour(BaseBehaviourOrchestrator self, Collision2D collision2D)
        {
            base.HandleBehaviour(self, collision2D);
            if (splitTimes > 0)
            {
                int childSplitTime = splitTimes - 1;
                for (int i = 0; i < splitNbr; i++)
                {
                    int half = splitNbr / 2;
                    GameObject split = GameObject.Instantiate(self.gameObject, self.gameObject.transform.position, Quaternion.Euler(0, 0, self.gameObject.transform.rotation.eulerAngles.z + (i - half) * 25));
                    OnAllBehaviourOrchestrator orchestrator = split.GetComponent<OnAllBehaviourOrchestrator>();
                    orchestrator.copyAllBehaviours(self);
                    // We exclude the target that already encountered the parent projectile
                    orchestrator.excludedTargets = ((OnAllBehaviourOrchestrator)self).alreadyTargeted;
                    orchestrator.getBehaviourByType<SplitBehaviour>().splitTimes = childSplitTime;
                }
            }
            return;
        }
    }
}