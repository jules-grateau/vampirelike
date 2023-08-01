﻿using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Items.Weapons;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui.Stats
{
    public class InGamePlayerListController : MonoBehaviour
    {
        private void Awake()
        {
            if (GameManager.GameState == null) return;

            StatListController statListController = gameObject.AddComponent<StatListController>();
            GameObject player = GameManager.GameState.Player;
            if (!player) return;
            CharacterStatisticsSO characterStatistics = player.GetComponent<PlayerStatsController>().CharacterStatistics;
            WeaponStatisticsSO weaponStatistics = player.GetComponent<WeaponInventoryManager>().WeaponStats;

            statListController.Init(characterStatistics, weaponStatistics);


        }
    }
}