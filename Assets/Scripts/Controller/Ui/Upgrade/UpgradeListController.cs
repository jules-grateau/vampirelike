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
        
        GameObject _upgradeInfoPrefab;

        public UpgradeManager UpgradeManager => _upgradeManager;
        UpgradeManager _upgradeManager;

        // Use this for initialization
        void Awake()
        {
            _upgradeManager = UpgradeManager.GetInstance(_upgradeType);
            _upgradeInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/UpgradeInfo");
            Draw((int)_numberSelectableUpgrade.value);
        }

        public void Redraw()
        {
            Draw((int)_numberSelectableUpgrade.value);
        }

        void Draw(int amount)
        {
            Clean();
            List<Upgrade<UpgradeSO>> upgrades = _upgradeManager.Draw(amount);
            _upgrades = new List<GameObject>();

            foreach (Upgrade<UpgradeSO> upgrade in upgrades)
            {
                GameObject upgradeInfo = Instantiate(_upgradeInfoPrefab, transform);
                UpgradeInfoController upgradeInfoController = upgradeInfo.GetComponent<UpgradeInfoController>();
                upgradeInfoController.Init(upgrade, _upgradeManager);
                _upgrades.Add(upgradeInfo);
            }

        }

        void Clean()
        {
            //Remove all child
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnEnable()
        {
            SetFirstUpgradeSelected();
        }

        public void SetFirstUpgradeSelected()
        {
            if (_upgrades.Count <= 0) return;

            EventSystem.current.SetSelectedGameObject(_upgrades[0]);
        }
    }
}