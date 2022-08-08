using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Weapon
{
    public abstract class Weapon : ScriptableObject
    {
        [SerializeField]
        protected float _cooldown;

        public float cooldown { get { return _cooldown; } }
        public abstract void Use(Vector2 holderPosition);
    }
}