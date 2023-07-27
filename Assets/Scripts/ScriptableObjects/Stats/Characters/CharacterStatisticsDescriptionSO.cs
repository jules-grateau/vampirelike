using Assets.Scripts.Types;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Characters
{
    [CreateAssetMenu(fileName = "Statistics", menuName = "Statistics/Player", order = 8)]
    public class StatisticSO : BaseStatisticDescriptionSO<StatisticEnum>
    {
    }
}