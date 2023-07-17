using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class ProjectileTrigger : MonoBehaviour
    {
        protected abstract void HandleEnemyCollision(Collider2D collider2D);

        protected void HandleWallCollision(Collider2D collider2D)
        {
            if (collider2D.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
                return;
            }
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            switch (collider2D.gameObject.tag)
            {
                case "Wall":
                    HandleWallCollision(collider2D);
                    return;
                case "Enemy":
                    HandleEnemyCollision(collider2D);
                    return;
                default:
                    return;
            }
        }
    }
}