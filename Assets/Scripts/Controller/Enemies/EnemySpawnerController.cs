﻿using Assets.Scripts.Controller.Game;
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
        private float _spawnCooldown = 5;

        [SerializeField]
        private Tilemap floor;

        private GameObject _player;

        private float _delay = 0;
        private float _radius = 0;

        private bool _forceSpawn;
        private int _phaseNbr = 0;
        private int _spawnedEnemy = 0;
        private int _currentWaveAmount = 0;

        private void Awake()
        {
            _radius = (Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height) / 1.5f;
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

            if ((_delay >= _spawnCooldown || _forceSpawn) && triggerSpawn)
            {
                // If it's a "normal" round for spawning ennemies
                if (!_forceSpawn)
                {
                    _phaseNbr++;
                    _currentWaveAmount = Mathf.FloorToInt(GameManager.GameState.DifficultyCurve.Evaluate(_phaseNbr));
                    _spawnedEnemy = 0;
                    Debug.Log("PHASE " + _phaseNbr + " STARTED -> need to generate " + _currentWaveAmount + " enemies");
                }

                // If there is still enemy missing to be spawned
                if(_spawnedEnemy < _currentWaveAmount)
                {
                    // Get random position
                    Vector2 playerPos = _player.transform.position;
                    Vector3 spawnPos = UnityEngine.Random.insideUnitCircle.normalized * _radius + playerPos;
                    bool isCorrectSpawn = floor.HasTile(Vector3Int.FloorToInt(spawnPos));

                    if (isCorrectSpawn)
                    {
                        // Pick random enemy
                        int random = UnityEngine.Random.Range(0, _enemies.Length);
                        Debug.DrawLine(_player.transform.position, spawnPos, Color.yellow, 2, false);
                        GameObject enemyGo = _enemies[random].GetEnemy();
                        enemyGo.transform.position = spawnPos;
                        enemyGo.transform.rotation = Quaternion.identity;
                        
                        enemyGo.SetActive(true);
                        _spawnedEnemy++;
                    }
                    _forceSpawn = true;
                }
                else
                {
                    Debug.Log("PHASE " + _phaseNbr + " ENDED -> generated " + _spawnedEnemy + " enemies");
                    _delay = 0;
                    _forceSpawn = false;
                }
            }  
            _delay += Time.deltaTime;
        }
    }
}