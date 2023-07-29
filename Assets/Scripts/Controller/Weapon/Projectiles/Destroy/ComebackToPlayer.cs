using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class ComebackToPlayer : MonoBehaviour
    {

        [SerializeField]
        public int numberOfHits;

        private ProjectileCollision _projectileCollision;
        private void Start()
        {
            _projectileCollision = GetComponent<ProjectileCollision>();
        }

        void Update()
        {
            if (_projectileCollision.alreadyTargeted.Count >= numberOfHits)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (_projectileCollision.parent.transform.position - transform.position)), 0.8f);
                var hits = Physics2D.OverlapCircle(transform.position, 0.1f, 1 << LayerMask.NameToLayer("Player"));
                if (!hits) return;
                Destroy(gameObject);
            }
        }
    }
}