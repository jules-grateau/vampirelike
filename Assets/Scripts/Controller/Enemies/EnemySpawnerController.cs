using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemySpawnerController : MonoBehaviour
    {
        [SerializeField]
        private bool triggerSpawn;
        [SerializeField]
        private GameObject[] enemyPrefab = Array.Empty<GameObject>();
        [SerializeField]
        private float spawnCooldown = 5;
        [SerializeField]
        private Tilemap floor;
        [SerializeField]
        private GameObject player;

        private float _delay = 0;
        private float _radius = 0;
        private bool _forceSpawn;

        private void Awake()
        {
            _radius = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height;
        }

        // Update is called once per frame
        void Update()
        {
            if((_delay >= spawnCooldown || _forceSpawn) && triggerSpawn)
            {
                // Get random position
                Vector2 playerPos = player.transform.position;
                Vector3 spawnPos = UnityEngine.Random.insideUnitCircle.normalized * _radius + playerPos;
                bool isCorrectSpawn = floor.HasTile(Vector3Int.FloorToInt(spawnPos));
                if (isCorrectSpawn)
                {
                    // Pick random enemy
                    int random = UnityEngine.Random.Range(0, enemyPrefab.Length);
                    Debug.DrawLine(player.transform.position, spawnPos, Color.green, 2, false);
                    Instantiate(enemyPrefab[random], spawnPos, Quaternion.identity);
                    _delay = 0;
                    _forceSpawn = false;
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