using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;
using Assets.Scripts.Types;

public class StraightMovementBehaviour : MovementBehaviour
{
    public override void HandleStartBehaviour(BaseBehaviourOrchestrator self)
    {
        triggeringStates = new ProjectileState[] { ProjectileState.Start };
    }
    public override void HandleBehaviour(BaseBehaviourOrchestrator self, float time)
    {
        self.gameObject.GetComponent<Rigidbody2D>().velocity = self.transform.right * speed;
    }
}
