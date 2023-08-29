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
using System.Linq;

namespace Assets.Scripts.Controller.Game
{
    [Serializable]
    public class LootTable
    {
        public List<Loot> loots;
        [Range(0.0F, 1.0F)]
        public float probability;
    }

    public class KeyController : MonoBehaviour
    {

        private static KeyController _instance;
        public static KeyController Instance => _instance;

        private WorldGenerator _worldGenerator;
        private float _radius = 0;
        [SerializeField]
        private float _maxSpawnRadius = 250;

        GameObject _chestRef;

        private Queue<GameObject> chestQueue = new Queue<GameObject>();

        [SerializeField]
        public List<LootTable> LootTables;

        void Awake()
        {
            _worldGenerator = gameObject.GetComponent<WorldGenerator>();
            _chestRef = Resources.Load<GameObject>("Prefabs/Props/Persistant/chest_1");
            _radius = Mathf.Abs(Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height);
            _instance = this;
        }

        void Update()
        {
            if (GameManager.GameState.State == Types.GameStateEnum.PAUSE) return;
            if (!GameManager.GameState.Player) return;
            if (chestQueue.Count <= 0) return;
            if (!_worldGenerator) return;

            GameObject chestToSpawn = chestQueue.Dequeue();
            GameObject _player = GameManager.GameState.Player;
            Vector2 playerPos = _player.transform.position;
            Vector3 spawnPos = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(_radius, _maxSpawnRadius) + playerPos;
            bool isCorrectSpawn = _worldGenerator.IsOnFloor(spawnPos);
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

        private List<Loot> getRandomDrop()
        {
            float rand = UnityEngine.Random.value;
            List<LootTable> canDrop = LootTables
                .OrderBy(lt => lt.probability)
                .Where(lt => lt.probability <= rand)
                .ToList();

            int random = UnityEngine.Random.Range(0, canDrop.Count);
            return canDrop[random].loots;
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

            ChestInteractibleController chestInteractible = chest.GetComponent<ChestInteractibleController>();
            chestInteractible.SetLoots(getRandomDrop());
            chestQueue.Enqueue(chest);

            return interactable;
        }
    }
}