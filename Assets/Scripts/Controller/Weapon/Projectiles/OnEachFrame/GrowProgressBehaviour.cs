using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;
using Assets.Scripts.Types;

[RequireComponent(typeof(Rigidbody2D))]
public class GrowProgressBehaviour : ProgressBehaviour
{
    [SerializeField]
    public Vector3 growValue;

    public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
    {
        triggeringState = ProjectileState.Start;
    }
    public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
    {
        self.transform.localScale += growValue;
    }
}
