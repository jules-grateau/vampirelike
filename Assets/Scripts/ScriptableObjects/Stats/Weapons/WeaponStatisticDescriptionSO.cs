using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "WeaponStatistics", menuName = "Statistics/Weapons", order = 8)]
    public class WeaponStatisticDescriptionSO : BaseStatisticDescriptionSO<WeaponStatisticEnum>
    {
    }
}