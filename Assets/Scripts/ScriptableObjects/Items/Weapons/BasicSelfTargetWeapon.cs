using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Self Target Weapon", menuName = "Weapon/SelfTargetWeapon", order = 2)]
    public class BasicSelfTargetWeapon : ProjectileWeapon
    {
        [SerializeField]
        private Vector2 _offset;

        public override void Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            GameObject target = GetTarget(holderPosition);
            if (!target) return;

            Vector2 targetPosition = target.transform.position;
            Vector2 aimDirection = (targetPosition - holderPosition);
            aimDirection.Normalize();

            var projectile = GetProjectile();
            Vector2 newPos = (holderPosition + _offset - aimDirection);
            projectile.transform.position = newPos;
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (holderPosition - newPos));
            projectile.SetActive(true);
        }

        private GameObject GetTarget(Vector2 shootFrom)
        {
            var hits = Physics2D.OverlapCircleAll(shootFrom, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy"));

            if (hits.Length <= 0) return null;

            var distance = Vector2.Distance(shootFrom, hits[0].transform.position);
            var target = hits[0].transform.gameObject;
            foreach (var hit in hits)
            {
                float newDist = Vector2.Distance(shootFrom, hit.transform.position);
                if (distance > newDist)
                {
                    distance = newDist;
                    target = hit.transform.gameObject;
                }
            }

            Debug.DrawLine(shootFrom, target.transform.position, Color.red, 2, false);
            return target;
        }
    }
}