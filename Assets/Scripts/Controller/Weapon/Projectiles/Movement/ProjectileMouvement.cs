using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ProjectileMouvement : MonoBehaviour
    {
        public float speed { get; set; }

        // Update is called once per frame
        void Update()
        {
            HandleProjectileMouvement();
        }

        public abstract void HandleProjectileMouvement();
    }
}