using UnityEditor;
using UnityEngine;
using Assets.Scripts.Types;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Self Target Weapon", menuName = "Weapon/SelfTargetWeapon", order = 2)]
    public class BasicSelfTargetWeapon : ProjectileWeapon
    {
        [SerializeField]
        private Vector2 _offset;
        [SerializeField]
        private bool _shootFromBehind;

        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            int amount = Mathf.FloorToInt(GetStats(WeaponStatisticEnum.ProjectileNumber));

            Vector2 shootFrom = holderPosition;
            GameObject[] target = GetTargets(holderPosition, amount);
            if (target == null || target.Count() <= 0) return false;

            for (int i = 0; i < target.Count(); i++)
            {
                Vector2 targetPosition = target[i].transform.position;
                Vector2 aimDirection = (targetPosition - shootFrom);
                aimDirection.Normalize();

                var projectile = GetProjectile();
                Vector2 newPos = (shootFrom);
                if (_shootFromBehind)
                {
                    newPos -= aimDirection * _offset;
                }  else
                {
                    newPos += aimDirection * _offset;
                }

                projectile.transform.position = newPos;
                projectile.transform.right = aimDirection;

                projectile.SetActive(true);
            }
            return true;
        }

        private GameObject[] GetTargets(Vector2 shootFrom, int numberOfTargets)
        {
            var hits = Physics2D.OverlapCircleAll(shootFrom, GetStats(Types.WeaponStatisticEnum.Range), 1 << LayerMask.NameToLayer("Enemy"));
            if (hits.Length <= 0) return null;

            GameObject[] targets = hits.Select(hit => new {data = hit, distance = Vector2.Distance(shootFrom, hit.transform.position) })
                .Where(hit => Physics2D.Raycast(shootFrom, hit.data.transform.position, hit.distance, 1 << LayerMask.NameToLayer("Wall")).collider == null)
                .OrderBy(hit => hit.distance)
                .Select(hit => hit.data.gameObject)
                .Take(numberOfTargets)
                .ToArray();

            return targets;
        }
    }
}