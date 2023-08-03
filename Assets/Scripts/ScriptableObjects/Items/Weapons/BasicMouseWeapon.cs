using Assets.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Mouse weapon", menuName = "Weapon/MouseWeapon", order = 2)]
    public class BasicMouseWeapon : ProjectileWeapon
    {
        [SerializeField]
        private Vector2 _offset;

        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            Vector2 mousePosition = Mouse.current.position.value;
            Vector2 aimDirection = (mousePosition - holderPosition);
            aimDirection.Normalize();

            int amount = Mathf.FloorToInt(GetStats(WeaponStatisticEnum.ProjectileNumber));
            for (int i = 0; i < amount; i++)
            {
                var projectile = GetProjectile();
                Vector2 newPos = (holderPosition + _offset * aimDirection);
                projectile.transform.position = newPos;
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (newPos - holderPosition));
                projectile.SetActive(true);
            }

            return true;
        }
    }
}