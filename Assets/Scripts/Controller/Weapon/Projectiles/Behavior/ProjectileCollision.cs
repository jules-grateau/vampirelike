using System.Collections;
using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class ProjectileCollision : MonoBehaviour, IDamageProjectile
    {
        [SerializeField]
        public bool bounceOnWall { get; set; }
        public GameObject parent { get; set; }
        [SerializeField]
        public float damage { get; set; }

        public List<GameObject> alreadyTargeted = new List<GameObject>();
        protected virtual void HandleEnemyCollision(Collision2D collision2D)
        {
            alreadyTargeted.Add(collision2D.gameObject);
        }

        protected void HandleWallCollision(Collision2D collision2D)
        {
            if (!bounceOnWall && collision2D.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
                return;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch(collision.gameObject.tag)
            {
                case "Wall":
                    HandleWallCollision(collision);
                    return;
                case "Enemy":
                    HandleEnemyCollision(collision);
                    return;
                default:
                    return;
            }
        }
    }
}