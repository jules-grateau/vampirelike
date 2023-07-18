using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Mouse weapon", menuName = "Weapon/MouseWeapon", order = 2)]
    public class BasicMouseWeapon : ProjectileWeapon
    {
        [SerializeField]
        private Vector2 _offset;

        public override void Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 aimDirection = (mousePosition - holderPosition);
            aimDirection.Normalize();

            var projectile = GetProjectile();
            Vector2 newPos = (holderPosition + _offset * aimDirection);
            projectile.transform.position = newPos;
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (newPos - holderPosition));
            projectile.SetActive(true);
        }
    }
}