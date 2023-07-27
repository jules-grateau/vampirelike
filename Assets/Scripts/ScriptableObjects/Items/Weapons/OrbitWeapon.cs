using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Weapon with circular projectile that orbit", menuName = "Weapon/Orbit", order = 1)]
    public class OrbitWeapon : ProjectileWeapon
    {
        [SerializeField]
        private int _amount;
        [SerializeField]
        private int _radius;

        [System.NonSerialized]
        private float _localAngle = 0f;
        [System.NonSerialized]
        private List<GameObject> _projectiles = new List<GameObject>();

        public override void Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            SanitizeProjectiles();
            int count = _projectiles.Count;
            for (int i = 0; i < _amount - count; i++)
            {
                GameObject projectile = GetProjectile();
                _projectiles.Add(projectile);
            }
            for (int i = count - 1; i >= _amount; i--)
            {
                Destroy(_projectiles[i]);
                _projectiles.RemoveAt(i);
            }
            for (int i = 0; i < _amount; i++)
            {
                float angle = (i * Mathf.PI * 2f / _amount) + _localAngle;
                Vector2 newPos = holderPosition + new Vector2(
                    Mathf.Cos(angle) * _radius,
                    Mathf.Sin(angle) * _radius
                );
                _projectiles[i].transform.position = newPos;
                _projectiles[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, -90) * (newPos - holderPosition));
                _projectiles[i].transform.localScale = new Vector3(1f * (1 + (GetStats(WeaponStatisticEnum.Size) / 100)), 1f * (1 + (GetStats(WeaponStatisticEnum.Size) / 100)), 1f);
                _projectiles[i].SetActive(true);
            }
            _localAngle += Time.deltaTime * (GetStats(WeaponStatisticEnum.BaseSpeed) * (1 + GetStats(WeaponStatisticEnum.SpeedPercentage) / 100));

            if (_localAngle >= 360f)
                _localAngle = 0f;
        }

        // Get rid of all the "null" projectiles in the list
        void SanitizeProjectiles()
        {
            for(int i = _projectiles.Count -1 ; i >= 0; i--)
            {
                if (!_projectiles[i]) _projectiles.RemoveAt(i);
            }
        }
    }
}