using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.Controller.Upgrades;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Variables;
using System.Collections.Generic;
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

            List<UpgradeSO> upgrades = upgradeManager.Draw((int) _numberSelectableUpgrade.value);

            foreach (UpgradeSO upgrade in upgrades)
            {
                GameObject characterInfo = Instantiate(upgradeInfoPrefab, transform);
                UpgradeInfoController stageInfoController = characterInfo.GetComponent<UpgradeInfoController>();
                stageInfoController.Init(upgrade);
            }
        }
    }
}