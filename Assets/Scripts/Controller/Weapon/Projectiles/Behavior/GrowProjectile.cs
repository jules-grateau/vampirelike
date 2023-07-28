using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrowProjectile : ProjectileBehaviour
{
    [SerializeField]
    public Vector3 growValue;
    public override void HandleProjectileBehaviour()
    {
        gameObject.transform.localScale += growValue;
    }
}
