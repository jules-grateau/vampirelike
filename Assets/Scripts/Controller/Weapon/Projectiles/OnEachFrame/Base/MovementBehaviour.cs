using System.Collections;
using UnityEngine;
using Assets.Scripts.Events.TypedEvents;
using System.Collections.Generic;

namespace Assets.Scripts.Controller.Weapon.Projectiles
{
    public abstract class MovementBehaviour : BaseBehaviour<float>
    {
        [SerializeField]
        public float speed { get; set; }
    }
}