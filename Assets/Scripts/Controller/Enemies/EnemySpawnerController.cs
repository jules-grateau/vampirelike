using System;
using UnityEngine;

namespace Assets.Scripts.Controller.Enemies
{
    public class EnemySpawnerController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] enemyPrefab = Array.Empty<GameObject>();
        [SerializeField]
        private int spawnCooldown = 5;
        private float _delay = 0;

        // Update is called once per frame
        void Update()
        {
            Debug.Log(_delay);

            if(_delay >= spawnCooldown)
            {
                int random = UnityEngine.Random.Range(0, enemyPrefab.Length);
                Instantiate(enemyPrefab[random], transform);
                _delay = 0;
            }
            _delay += Time.deltaTime;
        }
    }
}