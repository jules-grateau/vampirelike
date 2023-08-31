using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.OnEachFrame;
using Assets.Scripts.Types;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Mine weapon", menuName = "Weapon/Mine", order = 1)]
    public class MineWeapon : WeaponSO
    {

        [Header("Animation")]
        [SerializeField]
        private bool _isAnimatedEnd;
        [SerializeField]
        private bool _isAnimatedHit;

        [SerializeField]
        private float _explosionRadius;
        [SerializeField]
        private GameObject _particles;

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

            onAllBehaviourOrchestrator.addOnCollisionBehaviour(new ExplodeBehaviour()
            {
                damage = _weaponStats.GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (_weaponStats.GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                status = _weaponStatus,
                enemyHitEvent = _enemyHitEvent,
                explosionRadius = _explosionRadius,
                particles = _particles.GetComponent<ParticleSystem>()
            });

            onAllBehaviourOrchestrator.addOnCollisionBehaviour(new SelfDestroyNbrOfHits()
            {
                numberOfHits = 1,
                TriggeredProjectileState = ProjectileState.End
            });

            return projectile;
        }

        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            GameObject projectile = GetProjectile();
            projectile.transform.position = holderPosition;
            projectile.SetActive(true);

            return true;
        }
    }
}