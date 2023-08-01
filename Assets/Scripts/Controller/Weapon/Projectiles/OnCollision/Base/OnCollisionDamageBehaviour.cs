using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class OnCollisionDamageBehaviour : BaseBehaviour<Collision2D>
    {
        [SerializeField]
        public GameEventHitData enemyHitEvent;

        public GameObject parent { get; set; }
        [SerializeField]
        public float damage { get; set; }
    }
}