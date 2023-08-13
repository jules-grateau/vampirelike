using Assets.Scripts.Controller.Weapon.Projectiles;
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

        public override void Init(BaseStatisticsSO<WeaponStatisticEnum> additionalStats = null)
        {
            base.Init(additionalStats);
        }
        protected GameObject GetProjectile()
        {
            var projectile = Instantiate(_projectilPrefab);
            projectile.SetActive(false);

            OnAllBehaviourOrchestrator onAllBehaviourOrchestrator = projectile.AddComponent<OnAllBehaviourOrchestrator>();
            onAllBehaviourOrchestrator.parent = parent;

            return projectile;
        }

        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {

            var hits = Physics2D.OverlapCircleAll(holderPosition, GetStats(Types.WeaponStatisticEnum.Range), 1 << LayerMask.NameToLayer("Enemy"));
            if (hits.Length <= 0) return false;

            //_currentAuraPrefab.GetComponentInChildren<Animator>().SetBool("isUse", true);

            GameObject[] targets = hits.Select(hit => new { data = hit, distance = Vector2.Distance(holderPosition, hit.transform.position) })
                .Where(hit => Physics2D.Raycast(holderPosition, hit.data.transform.position, hit.distance, 1 << LayerMask.NameToLayer("Wall")).collider == null)
                .OrderBy(hit => hit.distance)
                .Select(hit => hit.data.gameObject)
                .Take(Mathf.RoundToInt(GetStats(WeaponStatisticEnum.ProjectileNumber)))
                .ToArray();

            foreach (GameObject target in targets)
            {
                GameObject projectile = GetProjectile();
                projectile.transform.position = target.transform.position;
                projectile.SetActive(true);

                _enemyHitEvent.Raise(new HitData
                {
                    damage = GetStats(WeaponStatisticEnum.BaseDamage) * (1 + (GetStats(WeaponStatisticEnum.DamagePercentage) / 100)),
                    instanceID = target.GetInstanceID(),
                    position = target.transform.position,
                    source = parent
                });
            }

            return true;
        }
    }
}