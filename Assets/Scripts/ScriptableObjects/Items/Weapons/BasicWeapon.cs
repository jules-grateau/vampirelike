using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Basic weapon", menuName = "Weapon/Basic", order = 1)]
    public class BasicWeapon : ProjectileWeapon
    {
        [SerializeField]
        private Vector2 _offset;

        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            var projectile = GetProjectile();
            Vector2 newPos = (holderPosition + _offset * holderDirection);
            projectile.transform.position = newPos;
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (newPos - holderPosition));
            projectile.SetActive(true);

            return true;
        }
    }
}