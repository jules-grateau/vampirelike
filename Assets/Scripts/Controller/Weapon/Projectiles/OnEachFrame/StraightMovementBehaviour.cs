using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;

public class StraightMovementBehaviour : MovementBehaviour
{
    public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float time)
    {
    }
    public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float time)
    {
        self.gameObject.GetComponent<Rigidbody2D>().velocity = self.transform.right * speed;
    }
}
