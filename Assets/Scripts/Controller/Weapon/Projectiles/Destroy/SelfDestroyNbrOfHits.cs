using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class SelfDestroyNbrOfHits : MonoBehaviour
    {

        [SerializeField]
        public int numberOfHits;

        private List<GameObject> _alreadyTargeted;
        private void Start()
        {
            _alreadyTargeted = GetComponent<ProjectileCollision>().alreadyTargeted;
        }

        void Update()
        {
            if (_alreadyTargeted.Count >= numberOfHits)
            {
                Destroy(gameObject);
            }
        }
    }
}