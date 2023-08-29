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
        private Tilemap floor;

        private GameObject _player;

        private float _delay = 0;
        private float _radius = 0;

        private bool _forceSpawn = false;
        private int _phaseNbr = 0;
        private float _spawnedPowerValue = 0;
        private float _currentWaveAmount = 0;

        private void Awake()
        {
            _radius = Mathf.Abs((Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height) / 1.5f);
        }

        private void Start()
        {
            _player = GameManager.GameState.Player;

            _enemies = Resources.LoadAll<EnemySO>("ScriptableObjects/Enemy");
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.GameState.State == Types.GameStateEnum.PAUSE) return;
            if (!_player) return;

            float spawnCooldown = GameManager.GameState.SpawnCooldown;

            if ((_delay >= spawnCooldown || _forceSpawn) && triggerSpawn)
            {
                // If it's a "normal" round for spawning ennemies
                if (!_forceSpawn)
                {
                    _phaseNbr++;
                    _currentWaveAmount = GameManager.GameState.DifficultyCurve.Evaluate(_phaseNbr * spawnCooldown);
                    _spawnedPowerValue = 0;
                    Debug.Log("PHASE " + _phaseNbr + " STARTED -> need to generate " + _currentWaveAmount + " enemies");
                }

                // If there is still enemy missing to be spawned
                if(_spawnedPowerValue < _currentWaveAmount)
                {
                    // Get random position
                    Vector2 playerPos = _player.transform.position;
                    Vector3 spawnPos = UnityEngine.Random.insideUnitCircle.normalized * _radius + playerPos;

                    // Pick random enemy
                    int random = UnityEngine.Random.Range(0, _enemies.Length);
                    Vector2 enemySize = _enemies[random].GetSize();
                    Vector2 offset = _enemies[random].GetColliderOffset();

                    bool isCorrectSpawn = IsEnemySpawnCorrect(spawnPos, enemySize, offset);

                    if (isCorrectSpawn && (_enemies[random].health + _spawnedPowerValue) < (_currentWaveAmount + 10) )
                    {
                        Debug.DrawLine(_player.transform.position, spawnPos, Color.yellow, 2, false);
                        GameObject enemyGo = _enemies[random].GetEnemy();
                        enemyGo.transform.position = spawnPos;
                        enemyGo.transform.rotation = Quaternion.identity;
                        
                        enemyGo.SetActive(true);
                        _spawnedPowerValue += _enemies[random].health;
                    }
                    _forceSpawn = true;
                }
                else
                {
                    Debug.Log("PHASE " + _phaseNbr + " ENDED -> generated " + _spawnedPowerValue + " enemies");
                    _delay = 0;
                    _forceSpawn = false;
                }
            }  
            _delay += Time.deltaTime;
        }

        bool IsEnemySpawnCorrect(Vector3 spawnPos, Vector2 enemySize, Vector2 offset)
        {
            enemySize = enemySize / 2;
            spawnPos += (Vector3) offset;
            //If spawnPos is incorrect
            if (!floor.HasTile(Vector3Int.FloorToInt(spawnPos))) return false;

            //Otherwise, we check all extremity of the enemy
            Vector2 topLeft = new Vector2(spawnPos.x - enemySize.x, spawnPos.y + enemySize.y);
            Vector2 topRight = (Vector2) spawnPos + enemySize;
            Vector2 bottomLeft = (Vector2)spawnPos - enemySize;
            Vector2 bottomRight = new Vector2(spawnPos.x + enemySize.x, spawnPos.y - enemySize.y);

            Debug.DrawLine(topRight, topLeft, Color.red, 1f);
            Debug.DrawLine(topLeft, bottomLeft, Color.white, 1f);
            Debug.DrawLine(bottomLeft, bottomRight, Color.green, 1f);
            Debug.DrawLine(bottomRight, topRight, Color.blue, 1f);

            if (!floor.HasTile(Vector3Int.FloorToInt(topLeft))) return false;
            if (!floor.HasTile(Vector3Int.FloorToInt(topRight))) return false;
            if (!floor.HasTile(Vector3Int.FloorToInt(bottomLeft))) return false;
            if (!floor.HasTile(Vector3Int.FloorToInt(bottomRight))) return false;

            return true;
        }


    }
}