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
        int _currValue = 0;

        private void Start()
        {
            GameObject player = GameManager.GameState.Player;
            if (!player) return;

            _goldController = player.GetComponent<PlayerGold>();

            _text = GetComponentInChildren<TextMeshProUGUI>();
            _text.SetText(_goldController.Value.ToString());
        }

        private void Update()
        {
            int newValue = _goldController.Value;
            if (newValue == _currValue) return;

            UpdateText(newValue);
        }

        void UpdateText(int value)
        {
            _text.SetText(value.ToString());
            _currValue = value;
        } 
    }
}