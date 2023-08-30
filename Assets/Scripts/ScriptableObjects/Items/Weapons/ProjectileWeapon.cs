using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame;
using Assets.Scripts.Types;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    public abstract class ProjectileWeapon : WeaponSO
    {

        [Header("Animation")]
        [SerializeField]
        private bool _isAnimatedEnd;
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
        [DrawIf("damageType", ProjectileDamages.Explosion, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private float _explosionRadius;
        [DrawIf("damageType", ProjectileDamages.Explosion, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private GameObject _particles;

        [Header("Direction")]
        [SerializeField]
        public ProjectileDirection directionType;
        [DrawIf("directionType", ProjectileDirection.TurnAroundSpawnPosition, ComparisonType.Equals, DisablingType.DontDraw)]
        [SerializeField]
        private float _radiusPerSecond;

        [SerializeField]
        private bool _endOnWall;
        [SerializeField]
        private bool _restartOnWall;

        [Header("Destruction")]
        [SerializeField]
        public ProjectileDestruction[] destructionType;

        [Header("Restart")]
        [SerializeField]
        public ProjectileDestruction[] restartTriggerType;


        [SerializeField]
        private bool _comeBackToPlayer;

        protected GameObject GetProjectile()
        {
            var projectile = Instantiate(_projectilPrefab);
            projectile.SetActive(false);

            OnAllBehaviourOrchestrator onAllBehaviourOrchestrator = projectile.AddComponent<OnAllBehaviourOrchestrator>();
            onAllBehaviourOrchestrator.parent = Parent;

            if (_isAnimatedEnd)
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new AnimateOnEndBehaviour());
            }
            else
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new NoAnimateOnEndBehaviour());
            }

            if (_isAnimatedHit)
            {
                onAllBehaviourOrchestrator.addOnCollisionBehaviour(new AnimateOnHitBehaviour());
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
                case ProjectileDamages.Explosion:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new ExplodeBehaviour()
                    {
                        damage = _weaponStats.GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        status = _weaponStatus,
                        enemyHitEvent = _enemyHitEvent,
                        explosionRadius = _explosionRadius,
                        particles = _particles.GetComponent<ParticleSystem>()
                    });
                    break;
                case ProjectileDamages.Split:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new SplitBehaviour()
                    {
                        damage = _weaponStats.GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        status = _weaponStatus,
                        enemyHitEvent = _enemyHitEvent,
                        splitNbr = Mathf.FloorToInt(_weaponStats.GetStats(WeaponStatisticEnum.SplitNumber)),
                        splitTimes = Mathf.FloorToInt(_weaponStats.GetStats(WeaponStatisticEnum.SplitTimes))
                    });
                    break;
                case ProjectileDamages.Direct:
                default:
                    onAllBehaviourOrchestrator.addOnCollisionBehaviour(new DirectDamageBehaviour()
                    {
                        damage = _weaponStats.GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                        status = _weaponStatus,
                        enemyHitEvent = _enemyHitEvent
                    });
                    break;
            }

            switch (directionType)
            {
                case ProjectileDirection.Straight:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new StraightMovementBehaviour()
                    {
                        speed = _weaponStats.GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + _weaponStats.GetStats(WeaponStatisticEnum.SpeedPercentage) / 100)
                    });
                    break;
                case ProjectileDirection.AutoAimed:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new AimedMovementBehaviour()
                    {
                        speed = _weaponStats.GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + _weaponStats.GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                        radius = _weaponStats.GetStats(WeaponStatisticEnum.Radius)
                    });
                    break;
                case ProjectileDirection.Ricochet:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new RicochetMovementBehaviour()
                    {
                        speed = _weaponStats.GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + _weaponStats.GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                        radius = _weaponStats.GetStats(WeaponStatisticEnum.Radius)
                    });
                    break;
                case ProjectileDirection.TurnBackTowardPlayer:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new TurnTowardPlayerOnRestart()
                    {
                        Range = _weaponStats.GetStats(WeaponStatisticEnum.Range),
                        speed = _weaponStats.GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + _weaponStats.GetStats(WeaponStatisticEnum.SpeedPercentage) / 100)
                    });
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new StraightMovementBehaviour()
                    {
                        speed = _weaponStats.GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + _weaponStats.GetStats(WeaponStatisticEnum.SpeedPercentage) / 100)
                    });
                    break;
                case ProjectileDirection.TurnAroundSpawnPosition:
                    onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new TurnAroundSpawnPointBehavior()
                    {
                        RadiusPerSecond = _radiusPerSecond,
                        Duration = _weaponStats.GetStats(WeaponStatisticEnum.Duration),
                        BaseDuration = _weaponStats.GetStats(WeaponStatisticEnum.BaseDuration),
                        BaseSpeed= _weaponStats.GetStats(WeaponStatisticEnum.BaseSpeed),
                        SpeedPercentage = _weaponStats.GetStats(WeaponStatisticEnum.SpeedPercentage)
                    });
                    break;
                case ProjectileDirection.None:
                default:
                    break;
            }

            HandleProjectileDestructionTypes(onAllBehaviourOrchestrator, destructionType);

            HandleProjectileDestructionTypes(onAllBehaviourOrchestrator, restartTriggerType, ProjectileState.Restart);


            if (_comeBackToPlayer)
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new ComebackToPlayerBehaviour()
                {
                    speed = _weaponStats.GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + _weaponStats.GetStats(WeaponStatisticEnum.SpeedPercentage) / 100),
                });
            }
            else
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new DestroyBehaviour()
                {
                });
            }

            if (_endOnWall)
            {
                onAllBehaviourOrchestrator.addOnCollisionBehaviour(new EndOnWallHit()
                {
                });
            }

            if(_restartOnWall)
            {
                onAllBehaviourOrchestrator.addOnCollisionBehaviour(new EndOnWallHit()
                {
                    TriggeredProjectileState = ProjectileState.Restart
                });
            }

            projectile.transform.localScale = new Vector3(1f * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.Size)/100)), 1f * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.Size) / 100)), 1f);

            return projectile;
        }


        void HandleProjectileDestructionTypes(OnAllBehaviourOrchestrator onAllBehaviourOrchestrator, ProjectileDestruction[] destructionType, 
            ProjectileState destructionState = ProjectileState.End)
        {
            if (destructionType == null || destructionType.Length <= 0) return;

            if (destructionType.Contains(ProjectileDestruction.RandomAfterTime))
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new SelfDestroyRandomDelay()
                {
                    Duration = _weaponStats.GetStats(WeaponStatisticEnum.Duration),
                    BaseDuration = _weaponStats.GetStats(WeaponStatisticEnum.BaseDuration),
                    TriggeredProjectileState = destructionState
                });
            }

            if (destructionType.Contains(ProjectileDestruction.OnRangeReach))
            {
                onAllBehaviourOrchestrator.addOnEachFrameBehaviour(new SelfDestroyRange()
                {
                    Range = _weaponStats.GetStats(WeaponStatisticEnum.Range),
                    TriggeredProjectileState = destructionState
                });
            }

            if (destructionType.Contains(ProjectileDestruction.NbrOfHits))
            {
                onAllBehaviourOrchestrator.addOnCollisionBehaviour(new SelfDestroyNbrOfHits()
                {
                    numberOfHits = Mathf.FloorToInt(_weaponStats.GetStats(WeaponStatisticEnum.NbrOfHit)),
                    TriggeredProjectileState = destructionState
                });
            }

            if (destructionType.Contains(ProjectileDestruction.ReachPlayer))
            {
                onAllBehaviourOrchestrator.addOnCollisionBehaviour(new SelfDestroyOnPlayerReach()
                {
                    TriggeredProjectileState = destructionState
                });
            }
        }
    }
}