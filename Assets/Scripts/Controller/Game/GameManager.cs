using Assets.Scripts.Controller.Player;
using Assets.Scripts.Events;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controller.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        GameDataSO _gameData;
        [SerializeField]
        string _deathMenuSceneName = "DeathMenu";
        [SerializeField]
        string _upgradeMenuSceneName = "UpgradeMenu";
        [SerializeField]
        GameEvent _pauseEvent;

        [SerializeField]
        FloatVariable _gameTime;

        [SerializeField]
        private AnimationCurve _xpCurve;

        [SerializeField]
        private AnimationCurve _difficultyCurve;

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

            GameObject playerSpawnGO = GameObject.FindGameObjectWithTag("PlayerSpawn");
            if(!playerSpawnGO)
            {
                Debug.Log("Pas de PlayerSpawn dans la scene");
                return;
            }

            Vector3 playerSpawnPosition = playerSpawnGO.transform.position;

            GameState.Player = _gameData.PlayableCharacter.Init(playerSpawnPosition);
            GameState.XpCurve = _gameData.XpCurve;
            GameState.DifficultyCurve = _gameData.DifficultyCurve;
        }

        private void Update()
        {
            _gameTime.value += Time.deltaTime;
            _currGold = _playerGoldController.Value;
        }

        private void Start()
        {
            if (_gameData.PlayableCharacter && _gameData.PlayableCharacter.StartWeapons?.Length > 0)
            {
                PlayerCollect collectScript = GameState.Player.GetComponent<PlayerCollect>();

                foreach (WeaponSO weapon in _gameData.PlayableCharacter.StartWeapons)
                {
                    collectScript.PlayerGetWeaponEvent.Raise(weapon);
                }
            }
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
    }
}