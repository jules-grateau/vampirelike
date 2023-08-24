using Assets.Scripts.ScriptableObjects.Characters;
using System.Collections;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using UnityEngine.Playables;
using Assets.Scripts.Controller.Player;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.Types;
using UnityEngine.Localization;
using Assets.Scripts.Variables;
using Assets.Scripts.Events;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Controller.Collectible;
using UnityEngine.Events;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "PlayableCharacter", menuName = "PlayableCharacter", order = 1)]
    public class PlayableCharacterSO : ScriptableObject
    {
        public string Name => _name.GetLocalizedString();
        [SerializeField]
        LocalizedString _name;

        public Sprite Sprite => _sprite;
        [SerializeField]
        Sprite _sprite;

        public string Description => _description.GetLocalizedString();
        [SerializeField]
        LocalizedString _description;

        public GameObject Prefab => _prefab;
        [SerializeField]
        GameObject _prefab;
        public BaseStatistics<WeaponStatisticEnum> WeaponStatistics => _weaponStatistics; 
        [SerializeField]
        private BaseStatistics<WeaponStatisticEnum> _weaponStatistics;

        public BaseStatistics<CharacterStatisticEnum> CharacterStatistics => _characterStatistics;
        [SerializeField]
        private BaseStatistics<CharacterStatisticEnum> _characterStatistics;

        public WeaponSO[] StartWeapons => _startWeapons;
        [SerializeField]
        WeaponSO[] _startWeapons;

        [Header("Health")]
        [SerializeField]
        private AudioClip _hitAudioClip;
        [SerializeField]
        private AudioClip _armorAudioClip;
        [SerializeField]
        private Sprite _armorSprite;

        [Header("Experience")]
        [SerializeField]
        private FloatVariable _xp;
        [SerializeField]
        private FloatVariable _maxXP;
        [SerializeField]
        private IntVariable _currentLevel;
        [SerializeField]
        private GameObject _levelUpEffectGO;
        [SerializeField]
        private AudioClip _levelUpAudioClip;

        [Header("Events")]
        [SerializeField]
        private GameEvent _playerDeathEvent;
        [SerializeField]
        private GameEvent _playerLevelUpEvent;
        [SerializeField]
        private GameEventFloat _playerHealEvent;
        [SerializeField]
        private GameEventFloat _playerGetGoldEvent;
        [SerializeField]
        private GameEventFloat _playerAddXp;
        [SerializeField]
        private GameEventCollectible _playerGetXpEvent;
        [SerializeField]
        private GameEventWeapon _playerGetWeaponEvent;
        [SerializeField]
        private GameEventCollectible _playerGetKeyEvent;
        [SerializeField]
        private GameEventHitData _dealDamageToPlayer;
        [SerializeField]
        private GameEventUpgrade _selectUpgrade;
        [SerializeField]
        private GameEventCollectible _collectiblePickup;

        public GameObject Init(Vector3 spawnPosition)
        {
            GameObject playableCharacterInstance = Instantiate(_prefab, spawnPosition, _prefab.transform.rotation);
            playableCharacterInstance.SetActive(false);

            PlayerStatsController playerStatsController = playableCharacterInstance.AddComponent<PlayerStatsController>();
            _characterStatistics.Init();
            playerStatsController.Init(_characterStatistics);

            WeaponInventoryManager weaponInventoryManager = playableCharacterInstance.AddComponent<WeaponInventoryManager>();
            _weaponStatistics.Init();
            weaponInventoryManager.Init(_weaponStatistics);

            PlayerMovementController playerMovementController = playableCharacterInstance.AddComponent<PlayerMovementController>();

            PlayerHealth playerHealth = playableCharacterInstance.AddComponent<PlayerHealth>();
            playerHealth.HitAudioClip = _hitAudioClip;
            playerHealth.ArmorAudioClip = _armorAudioClip;
            playerHealth.ArmorSprite = _armorSprite;
            playerHealth.OnPlayerDeathEvent = _playerDeathEvent;

            PlayerXp playerXp = playableCharacterInstance.AddComponent<PlayerXp>();
            playerXp.Xp = _xp;
            playerXp.MaxXP = _maxXP;
            playerXp.CurrentLevel = _currentLevel;
            playerXp.LevelUpEffectGO = _levelUpEffectGO;
            playerXp.LevelUpAudioClip = _levelUpAudioClip;
            playerXp.PlayerLevelUpEvent = _playerLevelUpEvent;

            PlayerKey playerKey = playableCharacterInstance.AddComponent<PlayerKey>();
            PlayerGold playerGold = playableCharacterInstance.AddComponent<PlayerGold>();

            PlayerCollect playerCollect = playableCharacterInstance.AddComponent<PlayerCollect>();
            playerCollect.PlayerHealEvent = _playerHealEvent;
            playerCollect.PlayerGetGoldEvent = _playerGetGoldEvent;
            playerCollect.PlayerGetXpEvent = _playerGetXpEvent;
            playerCollect.PlayerGetWeaponEvent = _playerGetWeaponEvent;
            playerCollect.PlayerGetKeyEvent = _playerGetKeyEvent;

           GameEventListenerUpgrade listenerUpgrade = playableCharacterInstance.AddComponent<GameEventListenerUpgrade>();
            listenerUpgrade.GameEvent = _selectUpgrade;
            listenerUpgrade.UnityEvent = new UnityEvent<Upgrade<UpgradeSO>>();
            listenerUpgrade.UnityEvent.AddListener(playerStatsController.OnSelectUpgrade);
            listenerUpgrade.UnityEvent.AddListener(weaponInventoryManager.OnSelectUpgrade);
            listenerUpgrade.UnityEvent.AddListener(playerHealth.OnSelectUpgrade);

            GameEventListenerHitData listenerHitData = playableCharacterInstance.AddComponent<GameEventListenerHitData>();
            listenerHitData.GameEvent = _dealDamageToPlayer;
            listenerHitData.UnityEvent = new UnityEvent<HitData>();
            listenerHitData.UnityEvent.AddListener(playerHealth.TakeDamage);

            GameEventListenerCollectible listenerCollectible = playableCharacterInstance.AddComponent<GameEventListenerCollectible>();
            listenerCollectible.GameEvent = _collectiblePickup;
            listenerCollectible.UnityEvent = new UnityEvent<CollectibleItem>();
            listenerCollectible.UnityEvent.AddListener(playerCollect.OnGetCollectible);

            GameEventListenerCollectible listenerCollectKey = playableCharacterInstance.AddComponent<GameEventListenerCollectible>();
            listenerCollectKey.GameEvent = _playerGetKeyEvent;
            listenerCollectKey.UnityEvent = new UnityEvent<CollectibleItem>();
            listenerCollectKey.UnityEvent.AddListener(playerKey.OnPlayerCollectKey);

            GameEventListenerFloat listenerFloatHealth = playableCharacterInstance.AddComponent<GameEventListenerFloat>();
            listenerFloatHealth.GameEvent = _playerHealEvent;
            listenerFloatHealth.UnityEvent = new UnityEvent<float>();
            listenerFloatHealth.UnityEvent.AddListener(playerHealth.OnPlayerHeal);

            GameEventListenerFloat listenerFloatGold = playableCharacterInstance.AddComponent<GameEventListenerFloat>();
            listenerFloatGold.GameEvent = _playerGetGoldEvent;
            listenerFloatGold.UnityEvent = new UnityEvent<float>();
            listenerFloatGold.UnityEvent.AddListener(playerGold.OnGoldCollect);

            GameEventListenerFloat listenerFloatXp = playableCharacterInstance.AddComponent<GameEventListenerFloat>();
            listenerFloatXp.GameEvent = _playerAddXp;
            listenerFloatXp.UnityEvent = new UnityEvent<float>();
            listenerFloatXp.UnityEvent.AddListener(playerXp.OnPlayerGainXp);

            GameEventListenerWeapon listenerWeapon = playableCharacterInstance.AddComponent<GameEventListenerWeapon>();
            listenerWeapon.GameEvent = _playerGetWeaponEvent;
            listenerWeapon.UnityEvent = new UnityEvent<WeaponSO>();
            listenerWeapon.UnityEvent.AddListener(weaponInventoryManager.EquipWeapon);

            playableCharacterInstance.SetActive(true);
            return playableCharacterInstance;
        }
    }
}