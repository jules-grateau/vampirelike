using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System;
using System.Collections;
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

            OnUnpause();
        }

        private void Update()
        {
            _gameTime.value += Time.deltaTime;
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
    }

    public class GameState
    {
        public GameObject Player { get; set; }
        public GameStateEnum State { get; set; }
    }
}