using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Stage;
using Assets.Scripts.Variables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Controller.Inventory.Weapons;
using Assets.Scripts.Controller.Game;
using Assets.Scripts.ScriptableObjects.Items;

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
            float maxUpgrade = _numberSelectableUpgrade.value;

            // Retrieve list of weapon currently used by the player
            List<WeaponSO> weaponManager = GameManager.GameState.Player.GetComponent<WeaponInventoryManager>().Inventory.Select(weaponController => weaponController.weapon).ToList();
            if (upgrades.Count > maxUpgrade)
            {
                while (upgradesToShow.Count < maxUpgrade)
                {
                    int randomUpgradeIndex = Random.Range(0, upgrades.Count);
                    UpgradeSO upgrade = upgrades[randomUpgradeIndex];
                    switch (upgrade)
                    {
                        case SpecificWeaponStatsUpgradeSO s:
                            if (weaponManager.Contains(s.ForWeapon))
                            {
                                upgradesToShow.Add(upgrade);
                                upgrades.RemoveAt(randomUpgradeIndex);
                            }
                            break;
                        default:
                            upgradesToShow.Add(upgrade);
                            upgrades.RemoveAt(randomUpgradeIndex);
                            break;
                    }
                }
            }
            else
            {
                upgradesToShow = upgrades.ToList();
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