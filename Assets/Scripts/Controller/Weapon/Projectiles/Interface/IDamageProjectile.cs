using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles.Interface
{
    public interface IDamageProjectile 
    {
        public GameObject parent { get; set; }
        public float damage { get; set; }
        public bool bounceOnWall { get; set; }
    }
}