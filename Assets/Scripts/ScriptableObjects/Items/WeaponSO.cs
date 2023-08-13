using UnityEditor;
using UnityEngine;
using Assets.Scripts.Types;
using Assets.Scripts.Controller.Weapon.Projectiles;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Events.TypedEvents;

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
        public GameEventHitData _enemyHitEvent;
        [SerializeField]
        protected BaseStatistics<WeaponStatisticEnum> _weaponStats;

        public Sprite icon { get { return _icon; } }
        public abstract bool Use(Vector2 holderPosition, Vector2 holderDirection);
        public GameObject parent { get; set; }

        public void UpgradeStats(WeaponStatisticEnum statisticEnum, float value, AdditionTypes additionType, float maxValue)
        {
            if (_weaponStats == null) return;

            _weaponStats.UpgradeStats(statisticEnum, value, additionType, maxValue);
            OnUpgradeStats();
        }

        public virtual void Init(BaseStatistics<WeaponStatisticEnum> additionalStats)
        {
            _weaponStats.Init(additionalStats);
        }

        public virtual float GetCooldown()
        {
            float attackCooldown = _weaponStats.GetStats(WeaponStatisticEnum.AttackCooldown);
            float attackSpeed = _weaponStats.GetStats(WeaponStatisticEnum.AttackSpeed);

            return attackCooldown * (1 - (attackSpeed / 100));
        }

        // Function to override in Childs if behavior wanted on Upgrade
        protected virtual void OnUpgradeStats()
        {
            return;
        }
    }
}