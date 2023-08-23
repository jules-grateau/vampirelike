using Assets.Scripts.Controller.Game;
using Assets.Scripts.Controller.Player;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class GoldTracker : MonoBehaviour
    {
        PlayerGold _goldController;
        TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            GameObject player = GameManager.GameState.Player;
            if (!player) return;

            _goldController = player.GetComponent<PlayerGold>();
            _goldController.OnGoldChange += OnGoldChange;
            _text.SetText(_goldController.Value.ToString());
        }

        private void OnDisable()
        {
            if (!_goldController) return;
            _goldController.OnGoldChange -= OnGoldChange;
        }

        void OnGoldChange(int newValue)
        {
            _text.SetText(newValue.ToString());
        }
    }
}