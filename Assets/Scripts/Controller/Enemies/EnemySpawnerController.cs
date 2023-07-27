using Assets.Scripts.Controller.Game;
using Assets.Scripts.ScriptableObjects.Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Controller.Enemies
{
    [RequireComponent(typeof(GameManager))]
    public class EnemySpawnerController : MonoBehaviour
    {
        [SerializeField]
        private bool triggerSpawn;

        [SerializeField]
        private EnemySO[] _enemies;

        [SerializeField]
        private float spawnCooldown = 5;
        [SerializeField]
        private Tilemap floor;

        private GameObject _player;

        private GameManager _gameManager;

        private float _delay = 0;
        private float _radius = 0;
        private bool _forceSpawn;

        private void Awake()
        {
            _radius = (Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height) / 1.5f;
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _gameManager = GetComponent<GameManager>();
            _enemies = Resources.LoadAll<EnemySO>("ScriptableObjects/Enemy");
        }

        // Update is called once per frame
        void Update()
        {
            if (_gameManager.GameState == Types.GameStateEnum.PAUSE) return;
            if (!_player) return;

            if((_delay >= spawnCooldown || _forceSpawn) && triggerSpawn)
            {
                // Get random position
                Vector2 playerPos = _player.transform.position;
                Vector3 spawnPos = UnityEngine.Random.insideUnitCircle.normalized * _radius + playerPos;
                bool isCorrectSpawn = floor.HasTile(Vector3Int.FloorToInt(spawnPos));
                if (isCorrectSpawn)
                {
                    // Pick random enemy
                    int random = UnityEngine.Random.Range(0, _enemies.Length);
                    //Debug.DrawLine(_player.transform.position, spawnPos, Color.green, 2, false);
                    GameObject enemyGo = _enemies[random].GetEnemy();
                    enemyGo.transform.position = spawnPos;
                    enemyGo.transform.rotation = Quaternion.identity;
                    _delay = 0;
                    _forceSpawn = false;
                    enemyGo.SetActive(true);
                }
                else
                {
                    _forceSpawn = true;
                }  
            }
            _delay += Time.deltaTime;
        }
    }
}