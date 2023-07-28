using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "WeaponStatisticsDescription", menuName = "Statistics/WeaponStatisticsDescription", order = 8)]
    public class WeaponStatisticDescriptionSO : BaseStatisticDescriptionSO<WeaponStatisticEnum>
    {
    }
}