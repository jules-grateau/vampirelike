using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Events;
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
    }
}