using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Player
{
    [RequireComponent(typeof(PlayerStatsController))]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private bool _resetOnStart;
        public float Hp => _hp;
        private float _hp;

        [SerializeField]
        private GameEvent _onPlayerDeathEvent;
        private PlayerStatsController _playerStatsController;

        private void Start()
        {
            _playerStatsController = GetComponent<PlayerStatsController>();
            if (!_playerStatsController) return;

            CharacterStatisticsSO characterStatistics = _playerStatsController.CharacterStatistics;
            if (!characterStatistics) return;

            if (_resetOnStart) _hp = characterStatistics.GetStats(Types.StatisticEnum.MaxHp);
            Debug.Log($"HP : {_hp}");
        }

        public void TakeDamage(float damage)
        {
            _hp -= damage;
            if(_hp <= 0)
            {
                Die();
            } 
        }

        void Die()
        {
            Destroy(gameObject);
            _onPlayerDeathEvent.Raise();
        }

        public void OnSelectUpgrade(UpgradeSO upgrade)
        {
            if (upgrade is not CharacterStatsUpgradeSO) return;

            CharacterStatsUpgradeSO statsUpgrade = (CharacterStatsUpgradeSO) upgrade;

            if (statsUpgrade.StatsToUpgrade != Types.StatisticEnum.MaxHp) return;
            Debug.Log("Added HP");
            _hp += statsUpgrade.ValueToAdd;  
        }
    }
}