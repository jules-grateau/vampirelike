using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "CharacterStatistics", menuName = "Statistics/CharacterStatistics", order = 1)]
    public abstract class CharacterStatisticsSO : BaseStatisticsSO<CharacterStatisticEnum>
    {
    }
}