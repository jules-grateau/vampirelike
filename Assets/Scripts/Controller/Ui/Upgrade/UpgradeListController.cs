using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.Controller.Upgrades;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Types;
using Assets.Scripts.Variables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controller.Ui
{
    public class UpgradeListController : MonoBehaviour
    {
        [SerializeField]
        string _upgradeType;

        [SerializeField]
        FloatVariable _numberSelectableUpgrade;

        List<GameObject> _upgrades;
        // Use this for initialization
        void Awake()
        {
            UpgradeManager upgradeManager = UpgradeManager.GetInstance(_upgradeType);
            GameObject upgradeInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/UpgradeInfo");

            List<Upgrade<UpgradeSO>> upgrades = upgradeManager.Draw((int) _numberSelectableUpgrade.value);
            _upgrades = new List<GameObject>();

            foreach (Upgrade<UpgradeSO> upgrade in upgrades)
            {
                GameObject upgradeInfo = Instantiate(upgradeInfoPrefab, transform);
                UpgradeInfoController upgradeInfoController = upgradeInfo.GetComponent<UpgradeInfoController>();
                upgradeInfoController.Init(upgrade, upgradeManager);
                _upgrades.Add(upgradeInfo);
            }
        }

        private void OnEnable()
        {
            if (_upgrades.Count <= 0) return;

            EventSystem.current.SetSelectedGameObject(_upgrades[0]);
        }
    }
}