using Assets.Scripts.ScriptableObjects.Characters;
using System.Collections;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using UnityEngine.Playables;
using Assets.Scripts.Controller.Player;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.Types;

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

        public GameObject Prefab => _prefab;
        [SerializeField]
        GameObject _prefab;

        public BaseStatistics<WeaponStatisticEnum> WeaponStatistics => _weaponStatistics; 
        [SerializeField]
        private BaseStatistics<WeaponStatisticEnum> _weaponStatistics;

        public BaseStatistics<CharacterStatisticEnum> CharacterStatistics => _characterStatistics;
        [SerializeField]
        private BaseStatistics<CharacterStatisticEnum> _characterStatistics;

        public GameObject Init(Vector3 spawnPosition)
        {
            GameObject playableCharacterInstance = Instantiate(_prefab, spawnPosition, _prefab.transform.rotation);

            PlayerStatsController playerStatsController = playableCharacterInstance.GetComponent<PlayerStatsController>();
            if (!playerStatsController) return playableCharacterInstance;
            _characterStatistics.Init();
            playerStatsController.Init(_characterStatistics);

            WeaponInventoryManager weaponInventoryManager = playableCharacterInstance.GetComponent<WeaponInventoryManager>();
            if (!weaponInventoryManager) return playableCharacterInstance;
            _weaponStatistics.Init();
            weaponInventoryManager.Init(_weaponStatistics);

            return playableCharacterInstance;
        }
    }
}