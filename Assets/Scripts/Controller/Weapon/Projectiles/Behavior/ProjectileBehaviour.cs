using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ProjectileBehaviour : MonoBehaviour
    {
        // Update is called once per frame
        void FixedUpdate()
        {
            HandleProjectileBehaviour();
        }

        public abstract void HandleProjectileBehaviour();
    }
}