using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrowProjectile : ProjectileBehaviour
{
    [SerializeField]
    private Vector3 _growValue;
    public override void HandleProjectileBehaviour()
    {
        gameObject.transform.localScale += _growValue;
    }
}
