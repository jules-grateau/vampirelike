using Assets.Scripts.Controller.Player;
using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller.Game
{
    [RequireComponent(typeof(GameStatistics))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        GameDataManager _gameData;
        [SerializeField]
        string _deathMenuSceneName = "DeathMenu";
        [SerializeField]
        string _upgradeMenuSceneName = "UpgradeMenu";
        [SerializeField]
        GameEvent _pauseEvent;

        [SerializeField]
        FloatVariable _gameTime;

        [SerializeField]
        private IntVariable _globalGold;

        private PlayerGold _playerGoldController;

        private int _currGold;

        public static GameState GameState => _gameState;

        private static GameState _gameState;

        void Awake()
        {
            _gameState = new GameState();

            _gameTime.value = 0f;
            OnPause();

            GameState.Player = _gameData.GetInstance().PlayableCharacter.Init();
            GameState.XpCurve = _gameData.GetInstance().XpCurve;
            GameState.DifficultyCurve = _gameData.GetInstance().DifficultyCurve;
            GameState.SpawnCooldown = _gameData.GetInstance().SpawnCooldown;
        }

        private void Update()
        {
            _gameTime.value += Time.deltaTime;
            _currGold = _playerGoldController.Value;
        }

        private void Start()
        {
            WeaponSpawnedController weaponSpawner = gameObject.GetComponent<WeaponSpawnedController>();
            weaponSpawner.Init();

            if (_gameData.GetInstance().PlayableCharacter && _gameData.GetInstance().PlayableCharacter.StartWeapons?.Length > 0)
            {
                PlayerCollect collectScript = GameState.Player.GetComponent<PlayerCollect>();

                foreach (WeaponSO weapon in _gameData.GetInstance().PlayableCharacter.StartWeapons)
                {
                    collectScript.PlayerGetWeaponEvent.Raise(weapon);
                }
            }
            weaponSpawner.SpawnWeapons();

            _playerGoldController = GameState.Player.GetComponent<PlayerGold>();

            OnUnpause();
        }
        public void OnPause()
        {
            _gameState.State = GameStateEnum.PAUSE;
            Time.timeScale = 0;
        }

        public void OnUnpause()
        {
            _gameState.State = GameStateEnum.RUNNING;
            Time.timeScale = 1;
        }

        public void OnPlayerDeath()
        {
            Time.timeScale = 0;
            SceneManager.LoadScene(_deathMenuSceneName, LoadSceneMode.Additive);
        }

        public void OnPlayerLevelUp()
        {
            _pauseEvent.Raise();
            SceneManager.LoadScene(_upgradeMenuSceneName,LoadSceneMode.Additive);
        }

        private void OnDestroy()
        {

            _globalGold.value += _currGold;
        }
    }

    public class GameState
    {
        public GameObject Player { get; set; }
        public GameStateEnum State { get; set; }
        public AnimationCurve XpCurve { get; set; }
        public AnimationCurve DifficultyCurve { get; set; }
        public float SpawnCooldown { get; set; }
    }
}