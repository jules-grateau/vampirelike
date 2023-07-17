using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class WeaponSO : ScriptableObject
    {
        [SerializeField]
        protected float _cooldown;
        [SerializeField]
        protected Sprite _icon;

        public Sprite icon { get { return _icon; } }

        public float cooldown { get { return _cooldown; } }
        public abstract void Use(Vector2 holderPosition, Vector2 holderDirection);
    }
}