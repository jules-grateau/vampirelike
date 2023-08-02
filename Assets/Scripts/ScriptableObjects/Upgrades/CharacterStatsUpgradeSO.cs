using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterStatsUpgradeSO", menuName = "Upgrade/CharacterStats", order = 1)]
    public class CharacterStatsUpgradeSO : BaseStatsUpgradeSO<CharacterStatisticEnum>
    {
    }
}