using UnityEngine;

namespace Assets.Scripts.Controller.Weapon
{
    [CreateAssetMenu(fileName = "Weapon with circular projectile", menuName = "Weapon/Circular", order = 1)]
    public class CircularWeapon : ProjectileWeapon
    {
        [SerializeField]
        private int _amount;
        [SerializeField]
        private int _radius;

        public override void Use(Vector2 holderPosition)
        {
            
            for(int i = 0; i < _amount; i++)
            {
                GameObject projectile = GetProjectile();
                float angle = i * Mathf.PI * 2f / _amount;
                Vector2 newPos = holderPosition + new Vector2(Mathf.Cos(angle) * _radius, Mathf.Sin(angle) * _radius);
                projectile.transform.position = newPos;
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (newPos - holderPosition));
                projectile.SetActive(true);
            }
        }
    }
}