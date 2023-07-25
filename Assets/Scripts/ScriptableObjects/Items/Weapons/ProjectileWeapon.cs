using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    public abstract class ProjectileWeapon : WeaponSO
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
            IDamageProjectile damageScript = projectile.GetComponent<IDamageProjectile>();
            if(damageScript != null)
            {
                damageScript.damage = _damage;
                damageScript.parent = parent;
            }
            return projectile;
        }
    }
}