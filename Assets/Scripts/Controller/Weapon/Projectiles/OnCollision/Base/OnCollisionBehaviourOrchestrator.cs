using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public class OnCollisionBehaviourOrchestrator : BaseBehaviourOrchestrator<Collision2D>
    {
        public List<GameObject> alreadyTargeted = new List<GameObject>();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            alreadyTargeted.Add(collision.gameObject);
            HandleAllBehaviour(collision);
        }
    }
}