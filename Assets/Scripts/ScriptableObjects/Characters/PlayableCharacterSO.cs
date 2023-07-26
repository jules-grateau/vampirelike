using Assets.Scripts.ScriptableObjects.Characters;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "PlayableCharacter", menuName = "PlayableCharacter", order = 1)]
    public class PlayableCharacterSO : ScriptableObject
    {
        public string Name => _name;
        [SerializeField]
        string _name;

        public Sprite Sprite => _sprite;
        [SerializeField]
        Sprite _sprite;

        public string Description => _description;
        [SerializeField]
        string _description;

        [SerializeField]
        CharacterStatisticsSO _characterStatistics;

        public GameObject Prefab => _prefab;
        [SerializeField]
        GameObject _prefab;

        public void Init(Vector3 spawnPosition)
        {
            GameObject playableCharacterInstance = Instantiate(_prefab, spawnPosition, _prefab.transform.rotation);
            PlayerStatsController playerStatsController = playableCharacterInstance.GetComponent<PlayerStatsController>();

            if (!playerStatsController) return;

            playerStatsController.Init(_characterStatistics);
        }
    }
}