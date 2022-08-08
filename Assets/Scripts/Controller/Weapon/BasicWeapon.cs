using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon
{
    [CreateAssetMenu(fileName = "Basic weapon", menuName = "Weapon/Basic", order = 1)]
    public class BasicWeapon : ProjectileWeapon
    {
        [SerializeField]
        private Vector2 _offset;

        public override void Use(Vector2 holderPosition)
        {
            var projectile = GetProjectile();
            projectile.transform.position = holderPosition + _offset;
            projectile.transform.rotation = Quaternion.identity;
            projectile.SetActive(true);
        }
    }
}