using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Stage;
using Assets.Scripts.Variables;
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
            GameObject upgradeInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/UpgradeInfo");
            List<UpgradeSO> upgrades = new List<UpgradeSO>();
            foreach (string upgrade in _upgradeType.Split(";"))
            {
                upgrades.AddRange(Resources.LoadAll<UpgradeSO>($"ScriptableObjects/Upgrade/{upgrade}"));
            }
            List<UpgradeSO> upgradesToShow = new List<UpgradeSO>();
            List<int> selectedUpgradeIndex = new List<int>();
            float maxUpgrade = _numberSelectableUpgrade.value;

            if(upgrades.Count > maxUpgrade)
            {
                while(upgradesToShow.Count < maxUpgrade)
                {
                    int randomUpgradeIndex = Random.Range(0, upgrades.Count);
                    if (!selectedUpgradeIndex.Contains(randomUpgradeIndex))
                    {
                        UpgradeSO upgrade = upgrades[randomUpgradeIndex];
                        upgradesToShow.Add(upgrade);
                        selectedUpgradeIndex.Add(randomUpgradeIndex);
                    }
                }

            } else
            {
                upgradesToShow = upgrades.ToList();
            }


            foreach(UpgradeSO upgrade in upgradesToShow)
            {
                GameObject characterInfo = Instantiate(upgradeInfoPrefab, transform);
                UpgradeInfoController stageInfoController = characterInfo.GetComponent<UpgradeInfoController>();
                stageInfoController.Init(upgrade);
            }
        }
    }
}