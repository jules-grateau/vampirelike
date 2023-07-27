using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Self Target Weapon", menuName = "Weapon/SelfTargetWeapon", order = 2)]
    public class BasicSelfTargetWeapon : ProjectileWeapon
    {
        [SerializeField]
        private Vector2 _offset;
        [SerializeField]
        private bool _shootFromBehind;

        public override void Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            Vector2 shootFrom = holderPosition + _offset;
            GameObject target = GetTarget(holderPosition);
            if (!target) return;

            Vector2 targetPosition = target.transform.position;
            Vector2 aimDirection = (targetPosition - shootFrom)
                ;
            aimDirection.Normalize();

            var projectile = GetProjectile();
            Vector2 newPos = (shootFrom);
            if(_shootFromBehind)
            {
                newPos -= aimDirection;
            }

            projectile.transform.position = newPos;
            projectile.transform.right = aimDirection;

            projectile.SetActive(true);
        }

        private GameObject GetTarget(Vector2 shootFrom)
        {
            var hits = Physics2D.OverlapCircleAll(shootFrom, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy"));

            if (hits.Length <= 0) return null;

            var distance = Mathf.Infinity;
            GameObject target = null;

            foreach (var hit in hits)
            {
                float newDist = Vector2.Distance(shootFrom, hit.transform.position);
                RaycastHit2D raycastHit = Physics2D.Raycast(shootFrom, hit.transform.position, newDist, 1 << LayerMask.NameToLayer("Wall"));

                if (raycastHit.collider)
                {
                    Debug.Log(raycastHit.collider.tag);
                    Debug.DrawLine(shootFrom, hit.transform.position, Color.red, 2, false);
                    continue;
                }
                
                Debug.DrawLine(shootFrom, hit.transform.position, Color.blue, 2, false);
                if (distance > newDist)
                {
                    distance = newDist;
                    target = hit.transform.gameObject;
                }
            }

            if(target) Debug.DrawLine(shootFrom, target.transform.position, Color.green, 2, false);

            return target;
        }
    }
}