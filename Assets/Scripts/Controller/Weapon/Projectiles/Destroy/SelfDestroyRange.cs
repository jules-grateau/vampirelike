using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyRange : MonoBehaviour
    {
        public float Range { get; set; }
        [SerializeField]
        private float _range;

        private Vector3 initPosition;
        ProjectileCollision _projectileCollision;

        void Start()
        {
            initPosition = transform.position;
        }

        private void FixedUpdate() 
        {
            if(Vector2.Distance(initPosition,transform.position) > _range)
            {
                Destroy(this);
                return;
            }
        }

    }
}