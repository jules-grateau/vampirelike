using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RicochetProjectile : ProjectileMouvement
    {
        [SerializeField]
        public float radius;
        Rigidbody2D _rigidbody;
        private List<GameObject> _alreadyTargeted;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _alreadyTargeted = GetComponent<ProjectileCollision>().alreadyTargeted;
        }

        public override void HandleProjectileMouvement()
        {
            _rigidbody.velocity = transform.right * speed;
            GameObject target = GetTarget(transform.position, _alreadyTargeted);
            if (!target) return;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (target.transform.position - transform.position)), 0.8f);
        }

        private GameObject GetTarget(Vector2 shootFrom, List<GameObject> excludes)
        {
            var hits = Physics2D.OverlapCircleAll(shootFrom, radius, 1 << LayerMask.NameToLayer("Enemy"));
            if (hits.Length <= 0) return null;

            GameObject target = hits.Select(hit => new { data = hit, distance = Vector2.Distance(shootFrom, hit.transform.position) })
                .Where(hit => Physics2D.Raycast(shootFrom, hit.data.transform.position, hit.distance, 1 << LayerMask.NameToLayer("Wall")).collider == null && !excludes.Contains(hit.data.gameObject))
                .OrderBy(hit => hit.distance)
                .Select(hit => hit.data.gameObject)
                .FirstOrDefault();

            return target;
        }
    }
}