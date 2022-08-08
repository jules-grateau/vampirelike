using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class DirectDamageProjectile : DamageDealingCollision
    {
        protected override void HandleEnemyCollision(Collision2D collision2D)
        {
            Debug.Log($"Hit enemy for {damage} damages");
            Destroy(gameObject);
            return;
        }
    }
}