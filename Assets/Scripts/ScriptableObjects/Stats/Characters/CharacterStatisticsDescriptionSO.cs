using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "CharacterStatisticsDescription", menuName = "Statistics/CharacterStatisticsDescription", order = 8)]
    public class CharacterStatisticsDescriptionSO : BaseStatisticDescriptionSO<CharacterStatisticEnum>
    {
    }
}