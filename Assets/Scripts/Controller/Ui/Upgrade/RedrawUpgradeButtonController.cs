using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Player;
using Assets.Scripts.Controller.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui.Upgrade
{
    [RequireComponent(typeof(Button))]
    public class RedrawUpgradeButtonController : MonoBehaviour
    {
        int _currPrice = 0;

        PlayerGold _goldController;

        [SerializeField]
        TextMeshProUGUI _priceText;

        UpgradeListController _upgradeList;
        UpgradeManager _upgradeManager;

        Button _button;

        protected void Awake()
        {
            GameObject player = GameManager.GameState.Player;
            _upgradeList = transform.parent.GetComponentInChildren<UpgradeListController>();
            _upgradeManager = _upgradeList.UpgradeManager;

            _button = GetComponent<Button>();
            _button.interactable = false;
            _currPrice = _upgradeManager.GetRedrawInitialCost();
            if (!player) return;

            _goldController = player.GetComponent<PlayerGold>();
            _button.onClick.AddListener(Pay);



            HandleButtonState();
        }

        void HandleButtonState()
        {
            _priceText.SetText(_currPrice.ToString());
            bool canPurchase = _goldController.CanUse(_currPrice);
            bool isCurrentlySelected = EventSystem.current.currentSelectedGameObject == _button.gameObject;
            _button.interactable = canPurchase;

            if (canPurchase) return;

            _priceText.color = Color.red;
            _priceText.alpha = 0.5f;
            if(isCurrentlySelected)
            {
                _upgradeList.SetFirstUpgradeSelected();
            }
        }

        void Pay()
        {
            _goldController.UseGold(_currPrice);
            _currPrice = _upgradeManager.GetNextCost(_currPrice);
            HandleButtonState();
        }
    }
}