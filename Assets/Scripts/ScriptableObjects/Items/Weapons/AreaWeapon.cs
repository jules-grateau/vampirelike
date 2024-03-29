﻿using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Characters;
using System.Linq;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Area of effect weapon", menuName = "Weapon/Area", order = 1)]
    public class AreaWeapon : WeaponSO
    {
        [SerializeField]
        protected GameObject _auraPrefab;

        [System.NonSerialized]
        private GameObject _currentAuraPrefab;

        public override void Init(GameObject parent, BaseStatistics<WeaponStatisticEnum> additionalStats = null)
        {
            base.Init(parent, additionalStats);

            GameObject aura = Instantiate(_auraPrefab);
            aura.transform.position = Parent.transform.position;
            aura.transform.localScale = new Vector3(_weaponStats.GetStats(Types.WeaponStatisticEnum.Range)*2, _weaponStats.GetStats(Types.WeaponStatisticEnum.Range)*2, 1f);
            aura.transform.parent = Parent.transform;

            _currentAuraPrefab = aura;
        }
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
            _currentAuraPrefab.transform.localScale = new Vector3(_weaponStats.GetStats(Types.WeaponStatisticEnum.Range) * 2, _weaponStats.GetStats(Types.WeaponStatisticEnum.Range) * 2, 1f);

            var hits = Physics2D.OverlapCircleAll(holderPosition, _weaponStats.GetStats(Types.WeaponStatisticEnum.Range), 1 << LayerMask.NameToLayer("Enemy"));
            if (hits.Length <= 0) return false;

            _currentAuraPrefab.GetComponentInChildren<Animator>().SetBool("isUse", true);

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
                bool orientation = Parent.transform.position.x < target.transform.position.x;
                projectile.transform.localScale = new Vector3((orientation ? 1 : -1) * projectile.transform.localScale.x, projectile.transform.localScale.y, projectile.transform.localScale.z);
                projectile.SetActive(true);

                _enemyHitEvent.Raise(new HitData
                {
                    damage = _weaponStats.GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                    instanceID = target.GetInstanceID(),
                    position = target.transform.position,
                    source = Parent,
                    status = _weaponStatus,
                    weapon = this
                });
            }

            return true;
        }
    }
}