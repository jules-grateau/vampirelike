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

        public GameStateEnum GameState => _gameState;
        GameStateEnum _gameState = GameStateEnum.PAUSE;

        void Awake()
        {
            _gameTime.value = 0f;
            OnPause();

            GameObject playerSpawnGO = GameObject.FindGameObjectWithTag("PlayerSpawn");
            if(!playerSpawnGO)
            {
                Debug.Log("Pas de PlayerSpawn dans la scene");
                return;
            }

            Vector3 playerSpawnPosition = playerSpawnGO.transform.position;

            _gameData.PlayableCharacter.Init(playerSpawnPosition);

            OnUnpause();
        }

        private void Update()
        {
            _gameTime.value += Time.deltaTime;
        }
        public void OnPause()
        {
            _gameState = GameStateEnum.PAUSE;
            Time.timeScale = 0;
        }

        public void OnUnpause()
        {
            _gameState = GameStateEnum.RUNNING;
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
}