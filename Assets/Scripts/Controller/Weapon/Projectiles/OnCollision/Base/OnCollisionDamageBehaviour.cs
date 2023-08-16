using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Status;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class OnCollisionDamageBehaviour : BaseBehaviour<Collision2D>
    {
        [SerializeField]
        public GameEventHitData enemyHitEvent;
        [SerializeField]
        public StatusSO status;
        [SerializeField]
        public float damage { get; set; }
    }
}