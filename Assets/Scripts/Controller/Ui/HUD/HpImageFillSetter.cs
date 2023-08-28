using Assets.Scripts.Controller.Player;
using Assets.Scripts.ScriptableObjects.Characters;
using Assets.Scripts.Types;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui
{
    public class HpImageFillSetter : MonoBehaviour
    {
        PlayerStatsController _playerStatsController;
        PlayerHealth _playerHealth;
        private Image _image;
        [SerializeField]
        private Image _armorImage;
        [SerializeField]
        private float _min = 0;

        [SerializeField]
        private TextMeshProUGUI _displayText;

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player) return;

            _playerStatsController = player.GetComponent<PlayerStatsController>();
            _playerHealth = player.GetComponent<PlayerHealth>();
            _image = GetComponent<Image>();

        }

        private void Update()
        {
            if (!_playerStatsController || !_playerHealth) return;
            BaseStatistics<CharacterStatisticEnum> characterStatistics = _playerStatsController.CharacterStatistics;
            if (characterStatistics == null) return;
            float maxHp = characterStatistics.GetStats(Types.CharacterStatisticEnum.MaxHp);
            float maxShield = characterStatistics.GetStats(Types.CharacterStatisticEnum.Shield);

            _image.fillAmount = Mathf.Clamp01(
                Mathf.InverseLerp(_min, maxHp, _playerHealth.Health));
            _armorImage.fillAmount = Mathf.Clamp01(
                    Mathf.InverseLerp(_min, maxShield, _playerHealth.Shield));

            if (!_displayText) return;

            string armorText = _playerHealth.Shield > 0 ? " - (" + _playerHealth.Shield.ToString() + " / " + maxShield.ToString() + ")" : "";
            _displayText.SetText(_playerHealth.Health.ToString() + " / " + maxHp.ToString() + armorText);
        }
    }
}