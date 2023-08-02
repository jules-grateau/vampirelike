using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.Controller.Upgrades;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Variables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class UpgradeListController : MonoBehaviour
    {
        [SerializeField]
        string _upgradeType;

        [SerializeField]
        FloatVariable _numberSelectableUpgrade;
        // Use this for initialization
        void Awake()
        {
            UpgradeManager upgradeManager = UpgradeManager.GetInstance(_upgradeType);
            GameObject upgradeInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/UpgradeInfo");

            List<UpgradeSO> upgrades = upgradeManager.GetAvailableUpgrades();
            List<UpgradeSO> upgradesToShow = new List<UpgradeSO>();
            float maxUpgrade = _numberSelectableUpgrade.value;


            while (upgradesToShow.Count < maxUpgrade && upgrades.Count > 0)
            {
                int randomUpgradeIndex = UnityEngine.Random.Range(0, upgrades.Count);
                UpgradeSO upgrade = upgrades[randomUpgradeIndex];

                upgradesToShow.Add(upgrade);
                upgrades.RemoveAt(randomUpgradeIndex);
            }

            foreach (UpgradeSO upgrade in upgradesToShow)
            {
                GameObject characterInfo = Instantiate(upgradeInfoPrefab, transform);
                UpgradeInfoController stageInfoController = characterInfo.GetComponent<UpgradeInfoController>();
                stageInfoController.Init(upgrade);
            }
        }
    }
}