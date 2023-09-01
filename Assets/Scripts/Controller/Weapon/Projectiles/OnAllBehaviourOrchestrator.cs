using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class OnAllBehaviourOrchestrator : BaseBehaviourOrchestrator
    {
        public List<GameObject> alreadyTargeted = new List<GameObject>();
        public List<GameObject> excludedTargets = new List<GameObject>();

        private void Start()
        {
            HandleAllStartBehaviour();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player") return;
            if (excludedTargets.Contains(collision.gameObject)) return;
            if (collision.gameObject.tag == "Enemy")
            {
                alreadyTargeted.Add(collision.gameObject);
            }
            HandleAllOnCollisionBehaviour(collision);
        }

        void FixedUpdate()
        {
            HandleAllOnEachFrameBehaviour(Time.deltaTime);
        }
    }
}