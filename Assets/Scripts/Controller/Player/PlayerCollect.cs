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
        public GameEventFloat PlayerHealEvent;
        [SerializeField]
        public GameEventFloat PlayerGetGoldEvent;
        [SerializeField]
        public GameEventCollectible PlayerGetXpEvent;
        [SerializeField]
        public GameEventWeapon PlayerGetWeaponEvent;
        [SerializeField]
        public GameEventCollectible PlayerGetKeyEvent;


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
                PlayerGetWeaponEvent.Raise(weaponCollectible.Weapon);
                return;
            }

            if(collectibleItem is KeyCollectible)
            {
                PlayerGetKeyEvent.Raise(collectibleItem);
            }

            if(IsSameOrSubclass(collectibleItem, typeof(ValueCollectible)))
            {
                ValueCollectible valueCollectible = (ValueCollectible)collectibleItem;
                switch(valueCollectible.Type)
                {
                    case ValueCollectibleTypes.Health:
                        PlayerHealEvent.Raise(valueCollectible.Value);
                        break;
                    case ValueCollectibleTypes.Gold:
                        PlayerGetGoldEvent.Raise(valueCollectible.Value);
                        break;
                    case ValueCollectibleTypes.Xp:
                        PlayerGetXpEvent.Raise(collectibleItem);
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