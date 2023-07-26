using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Player
{
    [RequireComponent(typeof(PlayerStatsController))]
    public class PlayerCollect : MonoBehaviour
    {
        [SerializeField]
        private FloatVariable _radius;
        private PlayerStatsController _playerStatsController;

        private void Start()
        {
            _playerStatsController = GetComponent<PlayerStatsController>();
        }

        public float getRadius()
        {
            CharacterStatisticsSO characterStatistics = _playerStatsController.CharacterStatistics;

            if (!characterStatistics) return 0f;

            return characterStatistics.GetStats(Types.StatisticEnum.PickUpRadius);
        }
    }
}