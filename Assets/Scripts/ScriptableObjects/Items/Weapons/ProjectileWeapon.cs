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

        [Header("Destruction")]
        [SerializeField]
        public ProjectileDestruction DestructionType;
        [DrawIf("DestructionType", ProjectileDestruction.RandomAfterTime, ComparisonType.Equals, DisablingType.DontDraw)]
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
            directDamageProjectileScript.damage = GetStats(WeaponStatisticEnum.BaseDamage) * ( 1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100) );
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

            switch(DestructionType)
            {
                case ProjectileDestruction.RandomAfterTime:
                    SelfDestroyRandomDelay selfDestroyRandomDelayScript = projectile.AddComponent<SelfDestroyRandomDelay>();
                    selfDestroyRandomDelayScript.minDelay = _minMaxDelay.x * (1 + (GetStats(WeaponStatisticEnum.Range) / 100));
                    selfDestroyRandomDelayScript.maxDelay = _minMaxDelay.y * (1 + (GetStats(WeaponStatisticEnum.Range) / 100));
                    break;
                case ProjectileDestruction.DestroyOnRangeReach:
                    SelfDestroyRange selfDestroyRangeSript = projectile.AddComponent<SelfDestroyRange>();
                    selfDestroyRangeSript.Range = GetStats(WeaponStatisticEnum.Range);
                    break;
                case ProjectileDestruction.None:
                default:
                    break;
            }

            projectile.transform.localScale = new Vector3(1f * (1 + (GetStats(WeaponStatisticEnum.Size)/100)), 1f * (1 + (GetStats(WeaponStatisticEnum.Size) / 100)), 1f);

            return projectile;
        }
    }
}