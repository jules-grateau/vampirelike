using Assets.Scripts.Controller.Weapon.Projectiles;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon
{
    public abstract class ProjectileWeapon : Weapon
    {
        [SerializeField]
        private GameObject _projectilPrefab;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _damage;

        protected GameObject GetProjectile()
        {
            var projectile = Instantiate(_projectilPrefab);
            projectile.SetActive(false);
            ProjectileMouvement mouvementScript = projectile.GetComponent<ProjectileMouvement>();
            if(mouvementScript != null)
            {
                mouvementScript.speed = _speed;
            }
            DamageDealingCollision damageScript = projectile.GetComponent<DamageDealingCollision>();
            if(damageScript != null)
            {
                damageScript.damage = _damage;
            }
            return projectile;
        }
    }
}