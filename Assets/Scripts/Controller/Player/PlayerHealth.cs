using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
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
        private AudioClip _hitAudioClip;
        [SerializeField]
        private AudioClip _armorAudioClip;

        [SerializeField]
        private GameEvent _onPlayerDeathEvent;
        private BaseStatistics<CharacterStatisticEnum> _characterStatistics;
        private SpriteRenderer _spriteRenderer;

        private bool isInvincible;

        private void Start()
        {
            isInvincible = false;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            PlayerStatsController playerStatsController = GetComponent<PlayerStatsController>();
            if (!playerStatsController) return;

            _characterStatistics = playerStatsController.CharacterStatistics;
            if (_characterStatistics == null) return;

            if (_resetOnStart) _hp = _characterStatistics.GetStats(Types.CharacterStatisticEnum.MaxHp);
        }

        public void TakeDamage(float damage)
        {
            if (!isInvincible)
            {
                float armor = _characterStatistics.GetStats(Types.CharacterStatisticEnum.Armor);
                float computedDamage = damage - armor;

                if (computedDamage < 1)
                {
                    AudioSource.PlayClipAtPoint(_armorAudioClip, transform.position, 1);
                    return;
                }

                _hp -= computedDamage;
                if (_hp <= 0)
                {
                    Die();
                }
                else
                {
                    AudioSource.PlayClipAtPoint(_hitAudioClip, transform.position, 1);
                    StartCoroutine(triggerInvincibility());
                }
            }
        }

        public void OnPlayerHeal(float value)
        {
            _hp += value;
        }

        private IEnumerator triggerInvincibility()
        {
            isInvincible = true;
            _spriteRenderer.material.SetInt("_Hit", 1);
            yield return new WaitForSeconds(_characterStatistics.GetStats(Types.CharacterStatisticEnum.Invincibility));
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

            if (statsUpgrade.StatsToUpgrade != Types.CharacterStatisticEnum.MaxHp) return;

            _hp += statsUpgrade.ValueToAdd;  
        }
    }
}