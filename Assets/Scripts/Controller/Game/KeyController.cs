using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Controller.Collectible;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Controller.Game
{
    public class KeyController : MonoBehaviour
    {

        private static KeyController _instance;
        public static KeyController Instance => _instance;

        [SerializeField]
        public Tilemap floor;
        private float _radius = 0;

        GameObject _chestRef;

        private Queue<GameObject> chestQueue = new Queue<GameObject>();

        void Awake()
        {
            _chestRef = Resources.Load<GameObject>("Prefabs/Props/Persistant/chest_1");
            _radius = Mathf.Abs(Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height);
            _instance = this;
        }

        void Update()
        {
            if (GameManager.GameState.State == Types.GameStateEnum.PAUSE) return;
            if (!GameManager.GameState.Player) return;
            if (chestQueue.Count <= 0) return;

            GameObject chestToSpawn = chestQueue.Dequeue();
            GameObject _player = GameManager.GameState.Player;
            Vector2 playerPos = _player.transform.position;
            Vector3 spawnPos = UnityEngine.Random.insideUnitCircle.normalized * _radius + playerPos;
            bool isCorrectSpawn = floor.HasTile(Vector3Int.FloorToInt(spawnPos));
            if (isCorrectSpawn)
            {
                chestToSpawn.transform.position = spawnPos;
                chestToSpawn.SetActive(true);
            }
            else
            {
                chestQueue.Enqueue(chestToSpawn);
            }
        }

        public InteractibleController queuChestGen(KeyCollectible key)
        {
            // Define a random color for the key
            SpriteRenderer sprite = key.gameObject.GetComponent<SpriteRenderer>();
            Color randomColor = new Color(
                 UnityEngine.Random.Range(0f, 1f),
                 UnityEngine.Random.Range(0f, 1f),
                 UnityEngine.Random.Range(0f, 1f)
             );
            sprite.color = randomColor;
            key.Color = randomColor;

            GameObject chest = Instantiate(_chestRef, gameObject.transform);
            chest.SetActive(false);
            InteractibleController interactable = chest.GetComponent<InteractibleController>();
            interactable.SetKey(key);
            chestQueue.Enqueue(chest);

            return interactable;
        }
    }
}