using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Stage;
using Assets.Scripts.Types;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui.CharacterSelection
{
    public class UpgradeInfoController : MonoBehaviour
    {
        TextMeshProUGUI _title;
        Image _image;
        TextMeshProUGUI _description;
        Upgrade<UpgradeSO> _upgrade;

        [SerializeField]
        GameEventUpgrade _selectUpgradeEvent;

        public void Init(Upgrade<UpgradeSO> upgrade)
        {
            _title = transform.Find("Text/Title").GetComponent<TextMeshProUGUI>();
            _image = transform.Find("Icon/Image").GetComponent<Image>();
            _description = transform.Find("Text/Description").GetComponent<TextMeshProUGUI>();
            _upgrade = upgrade;
            string rangeText = "";
            if(upgrade.UpgradeSO is WeaponStatsUpgradeSO)
            {
                WeaponStatsUpgradeSO weapUpgrade = (WeaponStatsUpgradeSO) upgrade.UpgradeSO;
                if(weapUpgrade.DropFrom != weapUpgrade.DropUntil)
                {
                    rangeText = $"(Range :{weapUpgrade.DropFrom}-{weapUpgrade.DropUntil})";
                }
            }
            if (upgrade.UpgradeSO is CharacterStatsUpgradeSO)
            {
                CharacterStatsUpgradeSO characUpgrade = (CharacterStatsUpgradeSO)upgrade.UpgradeSO;
                if (characUpgrade.DropFrom != characUpgrade.DropUntil)
                {
                    rangeText = $"{characUpgrade.DropFrom} - {characUpgrade.DropUntil}";
                }
            }

            _title.text = upgrade.UpgradeSO.Title + (rangeText != "" ? $" {rangeText} " : "")+$" ({upgrade.UpgradeQuality})";
            _image.sprite = upgrade.UpgradeSO.Sprite;
            _description.text = upgrade.GetDescription();
            Image upgradeImage = GetComponent<Image>();

            switch (upgrade.UpgradeQuality)
            {
                case Types.UpgradeQuality.Rare:
                    upgradeImage.color = Color.green;
                    break;
                case Types.UpgradeQuality.Epic:
                    upgradeImage.color = Color.cyan;
                    break;
                case Types.UpgradeQuality.Legendary:
                    upgradeImage.color = Color.yellow;
                    break;
                case Types.UpgradeQuality.Common:
                default:
                    break;
            }

        }

        public void OnClick()
        {
            _selectUpgradeEvent.Raise(_upgrade);
        }

    }
}