using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class ProjectileCollision : MonoBehaviour
    {
        [SerializeField]
        private bool bounceOnWall;

        protected abstract void HandleEnemyCollision(Collision2D collision2D);

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