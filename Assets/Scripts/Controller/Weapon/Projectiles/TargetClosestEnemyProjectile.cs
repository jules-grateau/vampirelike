using System;
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

            var hits = Physics2D.CircleCastAll(transform.position,Mathf.Infinity, Vector2.right,Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy"));

            if (hits.Length < 0) return;

            var distance = hits[0].distance;
            var target = hits[0].transform.gameObject;
            foreach(var hit in hits)
            {
                if(distance > hit.distance)
                {
                    distance = hit.distance;
                    target = hits[0].transform.gameObject;
                }
            }
            
            Debug.Log($"Found {target.name}");
            _target = target;

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