using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class DamageDealingCollision : ProjectileCollision
    {
        public float damage { get; set; }
    }
}