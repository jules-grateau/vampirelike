﻿using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Characters;
using System.Linq;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Close effect weapon", menuName = "Weapon/Close", order = 1)]
    public class CloseWeapon : WeaponSO
    {
        [SerializeField]
        protected GameObject _attackPrefab;

        protected GameObject GetProjectile()
        {
            var projectile = Instantiate(_projectilPrefab);
            projectile.SetActive(false);

            OnAllBehaviourOrchestrator onAllBehaviourOrchestrator = projectile.AddComponent<OnAllBehaviourOrchestrator>();
            onAllBehaviourOrchestrator.parent = Parent;

            return projectile;
        }

        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {

            var hits = Physics2D.OverlapCircleAll(holderPosition, _weaponStats.GetStats(Types.WeaponStatisticEnum.Range), 1 << LayerMask.NameToLayer("Enemy"));
            if (hits.Length <= 0) return false;

            //_currentAuraPrefab.GetComponentInChildren<Animator>().SetBool("isUse", true);

            GameObject[] targets = hits.Select(hit => new { data = hit, distance = Vector2.Distance(holderPosition, hit.transform.position) })
                .Where(hit =>
                {
                    RaycastHit2D hasDirectPath = Physics2D.Raycast(holderPosition, (hit.data.transform.position - (Vector3)holderPosition).normalized, hit.distance, 1 << LayerMask.NameToLayer("Wall"));
                    if (hasDirectPath.collider)
                    {
                        Debug.DrawLine(holderPosition, hasDirectPath.point, Color.green, 1f);
                        Debug.DrawLine(hasDirectPath.point, hit.data.transform.position, Color.red, 1f);
                        return false;
                    };
                    Debug.DrawLine(holderPosition, hit.data.transform.position, Color.green, 1f);
                    return true;
                })
                .OrderBy(hit => hit.distance)
                .Select(hit => hit.data.gameObject)
                .Take(Mathf.RoundToInt(_weaponStats.GetStats(WeaponStatisticEnum.ProjectileNumber)))
                .ToArray();

            foreach (GameObject target in targets)
            {
                GameObject projectile = GetProjectile();
                projectile.transform.position = target.transform.position;
                projectile.SetActive(true);

                _enemyHitEvent.Raise(new HitData
                {
                    damage = _weaponStats.GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                    instanceID = target.GetInstanceID(),
                    position = target.transform.position,
                    source = Parent
                });
            }

            return true;
        }
    }
}