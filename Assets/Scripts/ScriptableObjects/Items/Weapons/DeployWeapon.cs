using UnityEditor;
using UnityEngine;
using Assets.Scripts.Types;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Deploy Weapon", menuName = "Weapon/Deploy", order = 1)]
    public class DeployWeapon : ProjectileWeapon
    {
        [SerializeField]
        float _delayBetweenDeployement;

        int _amountLeft = 0;
        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            int amount = Mathf.FloorToInt(_weaponStats.GetStats(WeaponStatisticEnum.ProjectileNumber));
            if (_amountLeft == 0) _amountLeft = amount;

            DeployProjectile(holderPosition);

            return true;
        }

        void DeployProjectile(Vector2 holderPosition)
        {
            var projectile = GetProjectile();
            Vector2 newPos = (holderPosition);
            projectile.transform.position = newPos;
            projectile.SetActive(true);

            _amountLeft -= 1;
        }

        public override float GetCooldown()
        {
            if (_amountLeft >= 1) return _delayBetweenDeployement;

            return base.GetCooldown();
        }
    }
}