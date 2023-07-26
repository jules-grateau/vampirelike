using Assets.Scripts.ScriptableObjects.Game;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        GameDataSO _gameData;

        void Awake()
        {
            Time.timeScale = 0;

            GameObject playerSpawnGO = GameObject.FindGameObjectWithTag("PlayerSpawn");
            if(!playerSpawnGO)
            {
                Debug.Log("Pas de PlayerSpawn dans la scene");
                return;
            }

            Vector3 playerSpawnPosition = playerSpawnGO.transform.position;

            Instantiate(_gameData.PlayerPrefab, playerSpawnPosition,_gameData.PlayerPrefab.transform.rotation);

            Time.timeScale = 1;
        }
        public void OnPause()
        {
            Time.timeScale = 0;
        }

        public void OnUnpause()
        {
            Time.timeScale = 1;
        }
    }
}