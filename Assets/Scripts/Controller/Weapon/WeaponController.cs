using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponSO weapon;
    private Vector2 _lastDirection = Vector2.right;
    private Rigidbody2D _parentRigidBody;

    private void Start()
    {
        _parentRigidBody = gameObject.GetComponentInParent<Rigidbody2D>();
        setLastDirection();

        //First shoot;
        weapon.cooloff = weapon.GetCooldown();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (weapon == null) return;

        setLastDirection();
        float weapCooldown = weapon.GetCooldown();

        if (weapon.cooloff >= weapCooldown)
        {
            bool didUse = weapon.Use(gameObject.transform.position, _lastDirection);
            if(didUse) weapon.cooloff = 0;
        }
        weapon.cooloff += Time.fixedDeltaTime;
    }

    void setLastDirection()
    {
        if (_parentRigidBody && _parentRigidBody.velocity != Vector2.zero)
        {
            _lastDirection = _parentRigidBody.velocity.normalized;
        }
    }
}
