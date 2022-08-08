using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StraightFowardProjectile : ProjectileMouvement
{
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void HandleProjectileMouvement()
    {
        _rigidbody.velocity = transform.right * speed;
    }
}
