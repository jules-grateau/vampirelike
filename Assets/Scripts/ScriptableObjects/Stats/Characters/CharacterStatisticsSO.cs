using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "CharacterStatistics", menuName = "CharacterStatistics", order = 1)]
    public class CharacterStatisticsSO : BaseStatisticsSO<StatisticEnum>
    {
    }
}