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
        public float Armor { get; private set; }


        private float _timeSinceLastHit = 0;
        private float _timeSinceLastArmorRicovery = 0;

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
            Armor = _characterStatistics.GetStats(CharacterStatisticEnum.Armor);
        }

        private void Update()
        {
            _timeSinceLastHit += Time.deltaTime;
            float armorRecoveryDelay = _characterStatistics.GetStats(CharacterStatisticEnum.ArmorRecoveryDelay);
            if (_timeSinceLastHit < armorRecoveryDelay) return;
            
            _timeSinceLastArmorRicovery += Time.deltaTime;
            float maxArmor = _characterStatistics.GetStats(CharacterStatisticEnum.Armor);
            if (Armor >= maxArmor) return;

            float armorRecoveryTickRate = _characterStatistics.GetStats(CharacterStatisticEnum.ArmorRecoveryTickRate);
            if (_timeSinceLastArmorRicovery < armorRecoveryTickRate) return;

            float armorPerTick = _characterStatistics.GetStats(CharacterStatisticEnum.ArmorPerTick);

            Armor += armorPerTick;
            if (Armor > maxArmor) Armor = maxArmor;
            _timeSinceLastArmorRicovery = 0;
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
            
                float computedDamage = hit.damage - Armor;
                if (computedDamage < 1)
                {
                    AudioSource.PlayClipAtPoint(ArmorAudioClip, transform.position, 1);
                    if(_currentBlockCoroutine != null)
                    {
                        StopCoroutine(_currentBlockCoroutine);
                    }
                    _currentBlockCoroutine = StartCoroutine(triggerBlock(ArmorAudioClip.length * 0.5f));
                    Armor -= hit.damage;
                    _timeSinceLastHit = 0;
                    _timeSinceLastArmorRicovery = 0;
                    return;
                }

                Armor = 0;
                DisplayDamage(computedDamage, false, hit.status);
                Health -= computedDamage;
                if (Health <= 0)
                {
                    HandlePlayerDeath();
                }
                else
                {
                    AudioSource.PlayClipAtPoint(HitAudioClip, transform.position, 1);
                    StartCoroutine(triggerInvincibility());
                }

                _timeSinceLastHit = 0;
                _timeSinceLastArmorRicovery = 0;
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
            Destroy(gameObject);
        }

        protected override void triggerBeforeDestroy()
        {
            OnPlayerDeathEvent.Raise();
        }
    }
}