using UnityEngine;
using Assets.Scripts.Types;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Weapon with circular projectile", menuName = "Weapon/Circular", order = 1)]
    public class CircularWeapon : ProjectileWeapon
    {
        public override bool Use(Vector2 holderPosition, Vector2 holderDirection)
        {
            int amount = Mathf.FloorToInt(GetStats(WeaponStatisticEnum.ProjectileNumber));
            for (int i = 0; i < amount; i++)
            {
                GameObject projectile = GetProjectile();
                float angle = i * Mathf.PI * 2f / amount;
                Vector2 newPos = holderPosition + new Vector2(Mathf.Cos(angle) * GetStats(WeaponStatisticEnum.Range), Mathf.Sin(angle) * GetStats(WeaponStatisticEnum.Range));
                projectile.transform.position = newPos;
                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, 90) * (newPos - holderPosition));
                projectile.SetActive(true);
            }

            return true;
        }
    }
}