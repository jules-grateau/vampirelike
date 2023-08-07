using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.ScriptableObjects.Stage;
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
        UpgradeSO _upgrade;

        [SerializeField]
        GameEventUpgrade _selectUpgradeEvent;

        public void Init(UpgradeSO upgrade)
        {
            _title = transform.Find("Text/Title").GetComponent<TextMeshProUGUI>();
            _image = transform.Find("Icon/Image").GetComponent<Image>();
            _description = transform.Find("Text/Description").GetComponent<TextMeshProUGUI>();
            _upgrade = upgrade;

            _title.text = upgrade.Title + $" ({upgrade.UpgradeQuality})";
            _image.sprite = upgrade.Sprite;
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