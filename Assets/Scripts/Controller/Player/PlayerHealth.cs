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
        public AudioClip HitAudioClip;
        [SerializeField]
        public AudioClip ArmorAudioClip;
        [SerializeField]
        public Sprite ArmorSprite;
        [SerializeField]
        public GameEvent OnPlayerDeathEvent;
        public float Shield { get; private set; }
        private float _timeSinceLastShieldHit = 0;
        private float _timeSinceLastShieldRicovery = 0;

        private float _timeSinceLastHealthHit = 0;
        private float _timeSinceLastHealthRicovery = 0;

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

            Health = _characterStatistics.GetStats(Types.CharacterStatisticEnum.MaxHp);
            Shield = _characterStatistics.GetStats(CharacterStatisticEnum.Shield);
        }

        private void Update()
        {
            checkRefreshShield();
            checkRefreshHealth();
        }

        private void checkRefreshHealth()
        {
            _timeSinceLastHealthHit += Time.deltaTime;
            float healthRecoveryDelay = _characterStatistics.GetStats(CharacterStatisticEnum.HealthRecoveryDelay);
            if (_timeSinceLastHealthHit < healthRecoveryDelay) return;

            _timeSinceLastHealthRicovery += Time.deltaTime;
            float maxHealth = _characterStatistics.GetStats(CharacterStatisticEnum.MaxHp);
            if (Health >= maxHealth) return;

            float HealthRecoveryTickRate = _characterStatistics.GetStats(CharacterStatisticEnum.HealthRecoveryTickRate);
            if (_timeSinceLastHealthRicovery < HealthRecoveryTickRate) return;

            float healthPerTick = _characterStatistics.GetStats(CharacterStatisticEnum.HealthPerTick);

            Health += healthPerTick;
            if (Health > maxHealth) Health = maxHealth;
            _timeSinceLastHealthRicovery = 0;
        }

        private void checkRefreshShield()
        {
            _timeSinceLastShieldHit += Time.deltaTime;
            float shieldRecoveryDelay = _characterStatistics.GetStats(CharacterStatisticEnum.ShieldRecoveryDelay);
            if (_timeSinceLastShieldHit < shieldRecoveryDelay) return;

            _timeSinceLastShieldRicovery += Time.deltaTime;
            float maxShield = _characterStatistics.GetStats(CharacterStatisticEnum.Shield);
            if (Shield >= maxShield) return;

            float shieldRecoveryTickRate = _characterStatistics.GetStats(CharacterStatisticEnum.ShieldRecoveryTickRate);
            if (_timeSinceLastShieldRicovery < shieldRecoveryTickRate) return;

            float armorPerTick = _characterStatistics.GetStats(CharacterStatisticEnum.ShieldPerTick);

            Shield += armorPerTick;
            if (Shield > maxShield) Shield = maxShield;
            _timeSinceLastShieldRicovery = 0;
        }

        private IEnumerator triggerBlock(float timer)
        {
            SpriteRenderer sp = _anchor.GetComponent<SpriteRenderer>();
            sp.sprite = ArmorSprite;
            yield return new WaitForSeconds(timer);
            sp.sprite = null;
        }

        protected override void TakeDamageEffect(HitData hit, bool isDoTTick = false)
        {
            if (!isInvincible) {

                float reducedDamage = hit.damage * ( 1 - (_characterStatistics.GetStats(CharacterStatisticEnum.Armor) / 100));
                float computedDamage = reducedDamage - Shield;
                if (computedDamage < 1)
                {
                    AudioSource.PlayClipAtPoint(ArmorAudioClip, transform.position, 1);
                    if(_currentBlockCoroutine != null)
                    {
                        StopCoroutine(_currentBlockCoroutine);
                    }
                    _currentBlockCoroutine = StartCoroutine(triggerBlock(ArmorAudioClip.length * 0.5f));
                    Shield -= reducedDamage;
                    _timeSinceLastShieldHit = 0;
                    _timeSinceLastShieldRicovery = 0;
                    return;
                }

                Shield = 0;
                DisplayDamage(computedDamage, false, hit.status);
                Health -= computedDamage;
                _timeSinceLastHealthHit = 0;
                _timeSinceLastHealthRicovery = 0;
                if (Health <= 0)
                {
                    HandlePlayerDeath();
                }
                else
                {
                    AudioSource.PlayClipAtPoint(HitAudioClip, transform.position, 1);
                    StartCoroutine(triggerInvincibility());
                }

                _timeSinceLastShieldHit = 0;
                _timeSinceLastShieldRicovery = 0;
            }
        }

        public void OnPlayerHeal(float value)
        {;
            float maxHp = _characterStatistics.GetStats(CharacterStatisticEnum.MaxHp);
            if (Health + value > maxHp)
            {
                Health = maxHp;
                return;
            }

            Health += value;
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
            gameObject.SetActive(false);
        }

        protected override void triggerBeforeDestroy()
        {
            OnPlayerDeathEvent.Raise();
        }
    }
}