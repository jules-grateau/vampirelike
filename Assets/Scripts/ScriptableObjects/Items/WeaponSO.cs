using UnityEditor;
using UnityEngine;
using Assets.Scripts.Types;
using Assets.Scripts.Controller.Weapon.Projectiles;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using Assets.Scripts.Events.TypedEvents;

namespace Assets.Scripts.ScriptableObjects.Items
{
    public abstract class WeaponSO : WeaponStatisticsSO
    {
        [Header("Display")]
        [SerializeField]
        protected GameObject _projectilPrefab;
        [SerializeField]
        protected Sprite _icon;
        [SerializeField]
        public GameEventHitData _enemyHitEvent;

        public Sprite icon { get { return _icon; } }
        public abstract bool Use(Vector2 holderPosition, Vector2 holderDirection);
        public GameObject parent { get; set; }
    }
}