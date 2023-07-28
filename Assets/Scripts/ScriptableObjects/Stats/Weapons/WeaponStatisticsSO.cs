using Assets.Scripts.Controller.Weapon.Projectiles;
using Assets.Scripts.Controller.Weapon.Projectiles.Interface;
using Assets.Scripts.Types;
using Assets.Scripts.Events.TypedEvents;
using System.Collections; 
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.ScriptableObjects.Characters;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    public abstract class WeaponStatisticsSO : BaseStatisticsSO<WeaponStatisticEnum>
    {
        public virtual float GetCooldown()
        {
            float attackCooldown = GetStats(WeaponStatisticEnum.AttackCooldown);
            float attackSpeed = GetStats(WeaponStatisticEnum.AttackSpeed);

            return attackCooldown * (1 - (attackSpeed / 100));
        }
    }
}