using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "WeaponStatistics", menuName = "Statistics/WeaponStatistics", order = 1)]
    public class WeaponStatisticsSO : BaseStatisticsSO<WeaponStatisticEnum>
    {
        public virtual float GetCooldown()
        {
            float attackCooldown = GetStats(WeaponStatisticEnum.AttackCooldown);
            float attackSpeed = GetStats(WeaponStatisticEnum.AttackSpeed);

            return attackCooldown * (1 - (attackSpeed / 100));
        }
    }
}