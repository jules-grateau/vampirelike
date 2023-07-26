using Assets.Scripts.Controller.Weapon.Projectiles;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StraightFowardProjectile : ProjectileMouvement
{
    protected Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void HandleProjectileMouvement()
    {
        if (Time.timeScale == 0) return;
        _rigidbody.velocity = transform.right * speed;
    }
}
