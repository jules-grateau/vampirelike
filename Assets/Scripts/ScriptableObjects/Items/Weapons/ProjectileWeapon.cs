using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame;
using Assets.Scripts.Types;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    public abstract class ProjectileWeapon : WeaponSO
    {

        [Header("Animation")]
        [SerializeField]
        private bool _isAnimatedHit;

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
        private float _duration;
        [DrawIf("damageType", ProjectileDamages.Explosion, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private float _explosionRadius;
        [DrawIf("damageType", ProjectileDamages.Explosion, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private GameObject _particles;
        [DrawIf("damageType", ProjectileDamages.Split, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private int _splitNbr;
        [DrawIf("damageType", ProjectileDamages.Split, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        public int splitTimes;

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

        [SerializeField]
        private bool _comeBackToPlayer;

        protected GameObject GetProjectile()
        {
            var projectile = Instantiate(_projectilPrefab);
            projectile.SetActive(false);

            OnAllBehaviourOrchestrator onAllBehaviourOrchestrator = projectile.AddComponent<OnAllBehaviourOrchestrator>();
            onAllBehaviourOrchestrator.parent = parent;

            if (_isAnimatedHit)
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new AnimateOnHitBehaviour());
            }
            else
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new NoAnimateOnHitBehaviour());
            }

            switch (behaviourType)
            {
                case ProjectileBehaviourEnum.Grow:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new GrowProgressBehaviour()
                    {
                        growValue = _growValue
                    });
                    break;
                default: break;
            }
            
            switch (damageType)
            {
                case ProjectileDamages.PerSecond:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new DoTBehaviour()
                    {
                        damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        enemyHitEvent = _enemyHitEvent,
                        duration = _duration
                    });
                    break;
                case ProjectileDamages.Explosion:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new ExplodeBehaviour()
                    {
                        damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        enemyHitEvent = _enemyHitEvent,
                        explosionRadius = _explosionRadius,
                        particles = _particles.GetComponent<ParticleSystem>()
                    });
                    break;
                case ProjectileDamages.Split:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new SplitBehaviour()
                    {
                        damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        enemyHitEvent = _enemyHitEvent,
                        splitNbr = _splitNbr,
                        splitTimes = splitTimes
                    });
                    break;
                case ProjectileDamages.Direct:
                default:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new DirectDamageBehaviour()
                    {
                        damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        enemyHitEvent = _enemyHitEvent
                    });
                    break;
            }

            switch (directionType)
            {
                case ProjectileDirection.Straight:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new StraightMovementBehaviour()
                    {
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100)
                    });
                    break;
                case ProjectileDirection.AutoAimed:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new AimedMovementBehaviour()
                    {
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                        radius = GetStats(WeaponStatisticEnum.Radius)
                    });
                    break;
                case ProjectileDirection.Ricochet:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new RicochetMovementBehaviour()
                    {
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                        radius = GetStats(WeaponStatisticEnum.Radius)
                    });
                    break;
                case ProjectileDirection.TurnBackTowardPlayer:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new TurnTowardPlayerOnRange()
                    {
                        Range = GetStats(WeaponStatisticEnum.Range),
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100)
                    });
                    break;
                case ProjectileDirection.None:
                default:
                    break;
            }

            switch(destructionType)
            {
                case ProjectileDestruction.RandomAfterTime:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new SelfDestroyRandomDelay()
                    {
                        minDelay = _minMaxDelay.x * (1 + (GetStats(WeaponStatisticEnum.Range) / 100)),
                        maxDelay = _minMaxDelay.y * (1 + (GetStats(WeaponStatisticEnum.Range) / 100))
                    });
                    break;
                case ProjectileDestruction.DestroyOnRangeReach:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new SelfDestroyRange()
                    {
                        Range = GetStats(WeaponStatisticEnum.Range)
                    });
                    break;
                case ProjectileDestruction.DestroyNbrOfHits:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new SelfDestroyNbrOfHits()
                    {
                        numberOfHits = Mathf.FloorToInt(GetStats(WeaponStatisticEnum.NbrOfHit))
                    });
                    break;
                case ProjectileDestruction.ReachPlayer:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new SelfDestroyOnPlayerReach()
                    {
                    });
                    break;
                case ProjectileDestruction.None:
                default:
                    break;
            }

            if (_comeBackToPlayer)
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new ComebackToPlayerBehaviour()
                {
                    speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                });
            }
            else
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new DestroyOnEndBehaviour()
                {
                });
            }

            if (!_bounceOnWall)
            {
                onAllBehaviourOrchestrator.addOnCollisionBehaviour(new EndOnWallHit()
                {
                });
            }

            projectile.transform.localScale = new Vector3(1f * (1 + (GetStats(WeaponStatisticEnum.Size)/100)), 1f * (1 + (GetStats(WeaponStatisticEnum.Size) / 100)), 1f);

            return projectile;
        }
    }
}