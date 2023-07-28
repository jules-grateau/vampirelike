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
        private CharacterStatisticsSO _characterStatistics;
        private SpriteRenderer _spriteRenderer;

        private bool isInvincible;

        private void Start()
        {
            isInvincible = false;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            PlayerStatsController playerStatsController = GetComponent<PlayerStatsController>();
            if (!playerStatsController) return;

            _characterStatistics = playerStatsController.CharacterStatistics;
            if (!_characterStatistics) return;

            if (_resetOnStart) _hp = _characterStatistics.GetStats(Types.StatisticEnum.MaxHp);
        }

        public void TakeDamage(float damage)
        {
            if (!isInvincible)
            {
                float armor = _characterStatistics.GetStats(Types.StatisticEnum.Armor);
                float computedDamage = damage - armor;

                if (computedDamage < 1) return;

                _hp -= computedDamage;
                if (_hp <= 0)
                {
                    Die();
                }
                else
                {
                    StartCoroutine(triggerInvincibility());
                }
            }
        }

        private IEnumerator triggerInvincibility()
        {
            isInvincible = true;
            _spriteRenderer.material.SetInt("_Hit", 1);
            yield return new WaitForSeconds(_characterStatistics.GetStats(Types.StatisticEnum.Invincibility));
            isInvincible = false;
            _spriteRenderer.material.SetInt("_Hit", 0);
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

            _hp += statsUpgrade.ValueToAdd;  
        }
    }
}