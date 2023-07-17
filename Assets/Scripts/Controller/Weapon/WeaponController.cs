using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponSO weapon;
    private float cooloff = 0f;
    private Vector2 _lastDirection = Vector2.right;
    private Rigidbody2D _parentRigidBody;

    private void Start()
    {
        _parentRigidBody = gameObject.GetComponentInParent<Rigidbody2D>();
        setLastDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon == null) return;

        setLastDirection();

        if (cooloff >= weapon.cooldown)
        {
            weapon.Use(gameObject.transform.position, _lastDirection);
            cooloff = 0;
        }
        cooloff += Time.deltaTime;
    }

    void setLastDirection()
    {
        if (_parentRigidBody && _parentRigidBody.velocity != Vector2.zero)
        {
            _lastDirection = _parentRigidBody.velocity.normalized;
        }
    }
}
