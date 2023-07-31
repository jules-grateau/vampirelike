using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using UnityEngine;

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
        [DrawIf("damageType", ProjectileDamages.Explosion, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private float _explosionRadius;
        [DrawIf("damageType", ProjectileDamages.Explosion, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private GameObject _particles;

        [Header("Direction")]
        [SerializeField]
        public ProjectileDirection directionType;

        [SerializeField]
        private bool _bounceOnWall;
        [DrawIf("directionType", ProjectileDirection.Ricochet, ComparisonType.NotEqual, DisablingType.DontDraw)]

        [Header("Destruction")]
        [SerializeField]
        public ProjectileDestruction destructionType;
        [DrawIf("destructionType", ProjectileDestruction.RandomAfterTime, ComparisonType.Equals, DisablingType.DontDraw)]
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
                case ProjectileDamages.Explosion:
                    directDamageProjectileScript = projectile.AddComponent<ExplodeDamageProjectile>();
                    ((ExplodeDamageProjectile)directDamageProjectileScript).enemyHitEvent = _enemyHitEvent;
                    ((ExplodeDamageProjectile)directDamageProjectileScript).explosionRadius = _explosionRadius;
                    ((ExplodeDamageProjectile)directDamageProjectileScript).particles = _particles.GetComponent<ParticleSystem>();
                    break;
                case ProjectileDamages.Direct:
                default:
                    directDamageProjectileScript = projectile.AddComponent<DirectDamageProjectile>();
                    ((DirectDamageProjectile)directDamageProjectileScript).enemyHitEvent = _enemyHitEvent;
                    break;
            }
            directDamageProjectileScript.damage = GetStats(WeaponStatisticEnum.BaseDamage) * ( 1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100) );
            directDamageProjectileScript.parent = parent;
            directDamageProjectileScript.bounceOnWall = _bounceOnWall;

            ProjectileMouvement mouvementScript;
            switch (directionType)
            {
                case ProjectileDirection.Straight:
                    mouvementScript = projectile.AddComponent<StraightFowardProjectile>();
                    mouvementScript.speed = GetStats(WeaponStatisticEnum.BaseSpeed) * ( 1 + GetStats(WeaponStatisticEnum.SpeedPercentage)/100);
                    break;
                case ProjectileDirection.AutoAimed:
                    mouvementScript = projectile.AddComponent<TargetClosestEnemyProjectile>();
                    mouvementScript.speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100);
                    ((TargetClosestEnemyProjectile)mouvementScript).radius = GetStats(WeaponStatisticEnum.Radius);
                    break;
                case ProjectileDirection.Ricochet:
                    mouvementScript = projectile.AddComponent<RicochetProjectile>();
                    mouvementScript.speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100);
                    ((RicochetProjectile)mouvementScript).radius = GetStats(WeaponStatisticEnum.Radius);
                    break;
                case ProjectileDirection.None:
                default:
                    break;
            }

            switch(destructionType)
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
                case ProjectileDestruction.DestroyNbrOfHits:
                    SelfDestroyNbrOfHits selfDestroyNbrOfHits = projectile.AddComponent<SelfDestroyNbrOfHits>();
                    selfDestroyNbrOfHits.numberOfHits = Mathf.FloorToInt(GetStats(WeaponStatisticEnum.NbrOfHit));
                    break;
                case ProjectileDestruction.ComebackToPlayer:
                    ComebackToPlayer comebackToPlayer = projectile.AddComponent<ComebackToPlayer>();
                    comebackToPlayer.numberOfHits = Mathf.FloorToInt(GetStats(WeaponStatisticEnum.NbrOfHit));
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