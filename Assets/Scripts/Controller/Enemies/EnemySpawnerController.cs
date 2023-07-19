using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemySpawnerController : MonoBehaviour
    {
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

        private void Awake()
        {
            _radius = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height;
        }

        // Update is called once per frame
        void Update()
        {
            if(_delay >= spawnCooldown)
            {
                // Get random position
                bool isCorrectSpawn = false;
                Vector3 spawnPos = Vector3.zero;
                while (!isCorrectSpawn)
                {
                    Vector2 playerPos = player.transform.position;
                    spawnPos = UnityEngine.Random.insideUnitCircle.normalized * _radius + playerPos;
                    isCorrectSpawn = floor.HasTile(Vector3Int.FloorToInt(spawnPos));
                }
                // Pick random enemy
                int random = UnityEngine.Random.Range(0, enemyPrefab.Length);
                Debug.DrawLine(player.transform.position, spawnPos, Color.green, 2, false);
                Instantiate(enemyPrefab[random], spawnPos, Quaternion.identity);
                _delay = 0;
            }
            _delay += Time.deltaTime;
        }
    }
}