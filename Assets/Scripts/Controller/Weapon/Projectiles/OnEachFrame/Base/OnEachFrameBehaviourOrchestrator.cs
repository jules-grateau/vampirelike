using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class OnEachFrameBehaviourOrchestrator : BaseBehaviourOrchestrator<float>
    {
        public List<GameObject> alreadyTargeted = new List<GameObject>();

        private void Start()
        {
            HandleAllStartBehaviour(Time.deltaTime);
        }

        void FixedUpdate()
        {
            HandleAllBehaviour(Time.deltaTime);
        }

    }
}