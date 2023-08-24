using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controller.Player
{
    [RequireComponent(typeof(PlayerStatsController))]
    public class PlayerCollect : MonoBehaviour
    {
        private PlayerStatsController _playerStatsController;

        [SerializeField]
        private GameEventFloat _playerHealEvent;
        [SerializeField]
        private GameEventFloat _playerGetGoldEvent;
        [SerializeField]
        private GameEventCollectible _playerGetXpEvent;

        public GameEventWeapon PlayerGetWeaponEvent => _playerGetWeaponEvent;
        [SerializeField]
        private GameEventWeapon _playerGetWeaponEvent;
        [SerializeField]
        private GameEventCollectible _playerGetKeyEvent;


        private void Start()
        {
            _playerStatsController = GetComponent<PlayerStatsController>();
        }


        private void Update()
        {
            BaseStatistics<CharacterStatisticEnum> characterStatistics = _playerStatsController.CharacterStatistics;
            if (characterStatistics == null) return;

            float radius = characterStatistics.GetStats(Types.CharacterStatisticEnum.PickUpRadius);
            var hits = Physics2D.OverlapCircleAll(transform.position, radius);
            Collider2D[] _collectibles =  hits.Where((hit) => hit.gameObject.CompareTag("Collectible")).ToArray();

            foreach(Collider2D _collectibleCollider in _collectibles)
            {
                CollectibleItem collectible = _collectibleCollider.gameObject.GetComponent<CollectibleItem>();
                if (!collectible) continue;

                collectible.Attract(transform);
            }
        }

        public void OnGetCollectible(CollectibleItem collectibleItem)
        {
            if(collectibleItem is WeaponCollectible)
            {
                WeaponCollectible weaponCollectible = (WeaponCollectible)collectibleItem;
                _playerGetWeaponEvent.Raise(weaponCollectible.Weapon);
                return;
            }

            if(collectibleItem is KeyCollectible)
            {
                _playerGetKeyEvent.Raise(collectibleItem);
            }

            if(IsSameOrSubclass(collectibleItem, typeof(ValueCollectible)))
            {
                ValueCollectible valueCollectible = (ValueCollectible)collectibleItem;
                switch(valueCollectible.Type)
                {
                    case ValueCollectibleTypes.Health:
                        _playerHealEvent.Raise(valueCollectible.Value);
                        break;
                    case ValueCollectibleTypes.Gold:
                        _playerGetGoldEvent.Raise(valueCollectible.Value);
                        break;
                    case ValueCollectibleTypes.Xp:
                        _playerGetXpEvent.Raise(collectibleItem);
                        break;
                }
                return;
            }

            if(IsSameOrSubclass(collectibleItem, typeof(PowerCollectible)))
            {
                PowerCollectible powerCollectible = (PowerCollectible)collectibleItem;
                powerCollectible.TriggerEffect(gameObject);
                return;
            }
        }

        public bool IsSameOrSubclass(object obj, Type type)
        {
            return obj.GetType().IsSubclassOf(type)
                   || obj.GetType() == type;
        }
    }
}