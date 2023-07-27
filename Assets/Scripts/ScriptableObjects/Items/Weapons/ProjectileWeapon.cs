using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Types;
using Assets.Scripts.Events.TypedEvents;
using System.Collections; 
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    public abstract class ProjectileWeapon : WeaponSO
    {

        [Header("Behaviour")]
        [SerializeField]
        public ProjectileBehaviourEnum behaviourType;
        [SerializeField]
        [DrawIf("behaviourType", ProjectileBehaviourEnum.Grow, ComparisonType.Equals, DisablingType.DontDraw)]
        private Vector3 _growValue;

        [Header("Damage")]
        [SerializeField]
        public ProjectileDamages damageType;
        [SerializeField]
        [DrawIf("damageType", ProjectileDamages.PerSecond, ComparisonType.Equals, DisablingType.DontDraw)]
        private float _tickSpeed;
        [SerializeField]
        public GameEventHitData _enemyHitEvent;

        [Header("Direction")]
        [SerializeField]
        public ProjectileDirection directionType;

        [SerializeField]
        private bool _bounceOnWall;
        [SerializeField]
        private bool _destroyOnHit;
        [SerializeField]
        public bool autoDestroy;
        [DrawIf("autoDestroy", true, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private Vector2 _minMaxDelay;

        protected GameObject GetProjectile()
        {
            var projectile = Instantiate(_projectilPrefab);
            projectile.SetActive(false);

            switch (behaviourType)
            {
                case ProjectileBehaviourEnum.Grow:
                    GrowProjectile growProjectileScript = projectile.AddComponent<GrowProjectile>();
                    growProjectileScript.growValue = _growValue;
                    break;
                default: break;
            }

            IDamageProjectile directDamageProjectileScript;
            switch (damageType)
            {
                case ProjectileDamages.PerSecond:
                    directDamageProjectileScript = projectile.AddComponent<DamagePerSecondeProjectile>();
                    ((DamagePerSecondeProjectile)directDamageProjectileScript).tickSpeed = _tickSpeed;
                    ((DamagePerSecondeProjectile)directDamageProjectileScript).enemyHitEvent = _enemyHitEvent;
                    break;
                case ProjectileDamages.Direct:
                default:
                    directDamageProjectileScript = projectile.AddComponent<DirectDamageProjectile>();
                    ((DirectDamageProjectile)directDamageProjectileScript).enemyHitEvent = _enemyHitEvent;
                    break;
            }
            directDamageProjectileScript.damage = GetStats(WeaponStatisticEnum.BaseDamage);
            directDamageProjectileScript.parent = parent;
            directDamageProjectileScript.destroyOnHit = _destroyOnHit;
            directDamageProjectileScript.bounceOnWall = _bounceOnWall;

            ProjectileMouvement mouvementScript;
            switch (directionType)
            {
                case ProjectileDirection.None:
                    break;
                case ProjectileDirection.Straight:
                    mouvementScript = projectile.AddComponent<StraightFowardProjectile>();
                    mouvementScript.speed = GetStats(WeaponStatisticEnum.BaseSpeed) * ( 1 + GetStats(WeaponStatisticEnum.SpeedPercentage)/100);
                    break;
                case ProjectileDirection.AutoAimed:
                default:
                    mouvementScript = projectile.AddComponent<TargetClosestEnemyProjectile>();
                    mouvementScript.speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100);
                    break;
            }

            if (autoDestroy)
            {
                SelfDestroyRandomDelay selfDestroyRandomDelayScript = projectile.AddComponent<SelfDestroyRandomDelay>();
                selfDestroyRandomDelayScript.minDelay = _minMaxDelay.x * (1 + (GetStats(WeaponStatisticEnum.Range) / 100));
                selfDestroyRandomDelayScript.maxDelay = _minMaxDelay.y * (1 + (GetStats(WeaponStatisticEnum.Range) / 100));
            }

            projectile.transform.localScale = new Vector3(1f * (1 + (GetStats(WeaponStatisticEnum.Size)/100)), 1f * (1 + (GetStats(WeaponStatisticEnum.Size) / 100)), 1f);

            return projectile;
        }
    }
}