using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Types;

public class ChangeOrchestratorState : MonoBehaviour
{
    public void ChangeState(ProjectileState state) {
        BaseBehaviourOrchestrator orchestrator = gameObject.transform.parent.GetComponent<BaseBehaviourOrchestrator>();
        if (orchestrator)
        {
            orchestrator.TriggerNewState(state);
        }
    }
}