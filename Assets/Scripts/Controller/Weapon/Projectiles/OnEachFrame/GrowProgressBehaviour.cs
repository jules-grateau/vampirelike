using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrowProgressBehaviour : ProgressBehaviour
{
    [SerializeField]
    public Vector3 growValue;

    public override void HandleStartBehaviour(BaseBehaviourOrchestrator<float> self, float time)
    {
    }
    public override void HandleBehaviour(BaseBehaviourOrchestrator<float> self, float time)
    {
        self.transform.localScale += growValue;
    }
}
