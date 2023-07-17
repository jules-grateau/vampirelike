using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TargetClosestEnemyProjectile : ProjectileMouvement
    {
        GameObject _target;
        Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            var hit = Physics2D.CircleCast(transform.position,Mathf.Infinity, Vector2.right,Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy"));
            if(hit.collider)
            {
                Debug.Log($"Found {hit.transform.gameObject.name}");
              _target = hit.transform.gameObject;
            }
        }
        public override void HandleProjectileMouvement()
        {
            if(_target != null )
            {
               transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (_target.transform.position - transform.position)),0.8f);
            }

            _rigidbody.velocity = transform.right * speed;
        }
    }
}