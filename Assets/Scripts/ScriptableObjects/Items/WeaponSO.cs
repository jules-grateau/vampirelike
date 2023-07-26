using UnityEditor;
using UnityEngine;
using Assets.Scripts.Controller.Weapon.Projectiles;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class WeaponSO : ScriptableObject
    {
        [Header("Display")]
        [SerializeField]
        protected GameObject _projectilPrefab;
        [SerializeField]
        protected Sprite _icon;
        [SerializeField]
        protected float _cooldown;

        public Sprite icon { get { return _icon; } }
        public float cooldown { get { return _cooldown; } }
        public abstract void Use(Vector2 holderPosition, Vector2 holderDirection);
        public GameObject parent { get; set; }
    }
}