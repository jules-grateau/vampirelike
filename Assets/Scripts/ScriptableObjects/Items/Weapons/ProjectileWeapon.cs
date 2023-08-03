using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame;
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

        [SerializeField]
        private bool _comeBackToPlayer;

        protected GameObject GetProjectile()
        {
            var projectile = Instantiate(_projectilPrefab);
            projectile.SetActive(false);

            OnEachFrameBehaviourOrchestrator onEachFrameBehaviourOrchestrator = projectile.AddComponent<OnEachFrameBehaviourOrchestrator>();
            OnCollisionBehaviourOrchestrator onCollisionBehaviourOrchestrator = projectile.AddComponent<OnCollisionBehaviourOrchestrator>();

            switch (behaviourType)
            {
                case ProjectileBehaviourEnum.Grow:
                    onEachFrameBehaviourOrchestrator.addBehaviour(new GrowProgressBehaviour()
                    {
                        growValue = _growValue
                    });
                    break;
                default: break;
            }

            
            switch (damageType)
            {
                case ProjectileDamages.PerSecond:
                    onCollisionBehaviourOrchestrator.addBehaviour(new DoTBehaviour()
                    {
                        damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        parent = parent,
                        enemyHitEvent = _enemyHitEvent,
                        tickSpeed = _tickSpeed
                    });
                    break;
                case ProjectileDamages.Explosion:
                    onCollisionBehaviourOrchestrator.addBehaviour(new ExplodeBehaviour()
                    {
                        damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        parent = parent,
                        enemyHitEvent = _enemyHitEvent,
                        explosionRadius = _explosionRadius,
                        particles = _particles.GetComponent<ParticleSystem>()
                    });
                    break;
                case ProjectileDamages.Direct:
                default:
                    onCollisionBehaviourOrchestrator.addBehaviour(new DirectDamageBehaviour()
                    {
                        damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        parent = parent,
                        enemyHitEvent = _enemyHitEvent
                    });
                    break;
            }

            switch (directionType)
            {
                case ProjectileDirection.Straight:
                    onEachFrameBehaviourOrchestrator.addBehaviour(new StraightMovementBehaviour()
                    {
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100)
                    });
                    break;
                case ProjectileDirection.AutoAimed:
                    onEachFrameBehaviourOrchestrator.addBehaviour(new AimedMovementBehaviour()
                    {
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                        radius = GetStats(WeaponStatisticEnum.Radius)
                    });
                    break;
                case ProjectileDirection.Ricochet:
                    onEachFrameBehaviourOrchestrator.addBehaviour(new RicochetMovementBehaviour()
                    {
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                        radius = GetStats(WeaponStatisticEnum.Radius)
                    });
                    break;
                case ProjectileDirection.TurnBackTowardPlayer:
                    onEachFrameBehaviourOrchestrator.addBehaviour(new TurnTowardPlayerOnRange()
                    {
                        Range = GetStats(WeaponStatisticEnum.Range),
                        speed = GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100)
                    });
                    onCollisionBehaviourOrchestrator.addBehaviour(new ComeBackToPlayerOnWallHit()
                    {
                        parent = parent
                    });
                    break;
                case ProjectileDirection.None:
                default:
                    break;
            }

            switch(destructionType)
            {
                case ProjectileDestruction.RandomAfterTime:
                    onEachFrameBehaviourOrchestrator.addBehaviour(new SelfDestroyRandomDelay()
                    {
                        minDelay = _minMaxDelay.x * (1 + (GetStats(WeaponStatisticEnum.Range) / 100)),
                        maxDelay = _minMaxDelay.y * (1 + (GetStats(WeaponStatisticEnum.Range) / 100))
                    });
                    break;
                case ProjectileDestruction.DestroyOnRangeReach:
                    onEachFrameBehaviourOrchestrator.addBehaviour(new SelfDestroyRange()
                    {
                        Range = GetStats(WeaponStatisticEnum.Range)
                    });
                    break;
                case ProjectileDestruction.DestroyNbrOfHits:
                    onCollisionBehaviourOrchestrator.addBehaviour(new SelfDestroyNbrOfHits()
                    {
                        parent = parent,
                        numberOfHits = Mathf.FloorToInt(GetStats(WeaponStatisticEnum.NbrOfHit))
                    });
                    break;
                case ProjectileDestruction.ReachPlayer:
                    onCollisionBehaviourOrchestrator.addBehaviour(new SelfDestroyOnPlayerReach()
                    {
                        parent = parent,
                    });
                    break;
                case ProjectileDestruction.None:
                default:
                    break;
            }

            if (_comeBackToPlayer)
            {
                onCollisionBehaviourOrchestrator.addBehaviour(new ComeBackToPlayerOnWallHit()
                {
                    parent = parent
                });
                onEachFrameBehaviourOrchestrator.addBehaviour(new ComeBackToPlayerOnRange()
                {
                    Range = GetStats(WeaponStatisticEnum.Range)
                });
            }

            if (!_bounceOnWall)
            {
                onCollisionBehaviourOrchestrator.addBehaviour(new EndOnWallHit()
                {
                    parent = parent,
                });
            }

            projectile.transform.localScale = new Vector3(1f * (1 + (GetStats(WeaponStatisticEnum.Size)/100)), 1f * (1 + (GetStats(WeaponStatisticEnum.Size) / 100)), 1f);

            return projectile;
        }
    }
}