using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using Assets.Scripts.Types.Upgrades;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Controller.Collectible;

namespace Assets.Scripts.Controller.Player
{
    [RequireComponent(typeof(PlayerStatsController))]
    public class PlayerHealth : BaseHealth
    {
        [SerializeField]
        private bool _resetOnStart;

        [SerializeField]
        private AudioClip _hitAudioClip;
        [SerializeField]
        private AudioClip _armorAudioClip;
        [SerializeField]
        private Sprite _armorSprite;

        [SerializeField]
        private GameEvent _onPlayerDeathEvent;
        private BaseStatistics<CharacterStatisticEnum> _characterStatistics;
        private SpriteRenderer _spriteRenderer;
        private Coroutine _currentBlockCoroutine;

        private bool isInvincible;

        public void Start()
        {
            isInvincible = false;

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            PlayerStatsController playerStatsController = GetComponent<PlayerStatsController>();
            if (!playerStatsController) return;

            _characterStatistics = playerStatsController.CharacterStatistics;
            if (_characterStatistics == null) return;

            if (_resetOnStart) Health = _characterStatistics.GetStats(Types.CharacterStatisticEnum.MaxHp);
        }

        private IEnumerator triggerBlock(float timer)
        {
            SpriteRenderer sp = _anchor.GetComponent<SpriteRenderer>();
            sp.sprite = _armorSprite;
            yield return new WaitForSeconds(timer);
            sp.sprite = null;
        }

        protected override void TakeDamageEffect(HitData hit, bool isDoTTick = false)
        {
            if (!isInvincible)
            {
                float armor = _characterStatistics.GetStats(Types.CharacterStatisticEnum.Armor);
                float computedDamage = hit.damage - armor;

                if (computedDamage < 1)
                {
                    AudioSource.PlayClipAtPoint(_armorAudioClip, transform.position, 1);
                    if(_currentBlockCoroutine != null)
                    {
                        StopCoroutine(_currentBlockCoroutine);
                    }
                    _currentBlockCoroutine = StartCoroutine(triggerBlock(_armorAudioClip.length * 0.5f));
                    return;
                }

                DisplayDamage(computedDamage, false, hit.status);
                Health -= computedDamage;
                if (Health <= 0)
                {
                    HandlePlayerDeath();
                }
                else
                {
                    AudioSource.PlayClipAtPoint(_hitAudioClip, transform.position, 1);
                    StartCoroutine(triggerInvincibility());
                }
            }
        }

        public void OnPlayerHeal(CollectibleItem collectible)
        {
            ValueCollectible healthCollectible = (ValueCollectible)collectible;
            float maxHp = _characterStatistics.GetStats(CharacterStatisticEnum.MaxHp);
            if (Health + healthCollectible.Value > maxHp)
            {
                Health = maxHp;
                return;
            }

            Health += healthCollectible.Value;
        }

        private IEnumerator triggerInvincibility()
        {
            isInvincible = true;
            _spriteRenderer.material.SetInt("_Hit", 1);
            yield return new WaitForSeconds(_characterStatistics.GetStats(Types.CharacterStatisticEnum.Invincibility));
            isInvincible = false;
            _spriteRenderer.material.SetInt("_Hit", 0);
        }

        public void OnSelectUpgrade(Upgrade<UpgradeSO> upgrade)
        {
            if (upgrade.UpgradeSO is not CharacterStatsUpgradeSO) return;

            CharacterStatsUpgrade statsUpgrade = new CharacterStatsUpgrade(upgrade.UpgradeQuality, (CharacterStatsUpgradeSO) upgrade.UpgradeSO);

            if (statsUpgrade.UpgradeSO.StatsToUpgrade != Types.CharacterStatisticEnum.MaxHp) return;

            Health += statsUpgrade.GetValue();  
        }

        void HandlePlayerDeath()
        {
            triggerBeforeDestroy();
            Destroy(gameObject);
        }

        protected override void triggerBeforeDestroy()
        {
            _onPlayerDeathEvent.Raise();
        }
    }
}