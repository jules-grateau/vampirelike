using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class OnAllBehaviourOrchestrator : BaseBehaviourOrchestrator
    {
        public List<GameObject> alreadyTargeted = new List<GameObject>();
        private void Start()
        {
            HandleAllStartBehaviour();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            alreadyTargeted.Add(collision.gameObject);
            HandleAllOnCollisionBehaviour(collision);
        }

        void FixedUpdate()
        {
            HandleAllOnEachFrameBehaviour(Time.deltaTime);
        }
    }
}