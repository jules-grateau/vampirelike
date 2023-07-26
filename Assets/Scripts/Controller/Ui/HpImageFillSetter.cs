using Assets.Scripts.Controller.Player;
using Assets.Scripts.ScriptableObjects.Characters;
using System.Collections;
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
        private float _min = 0;

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
            CharacterStatisticsSO characterStatistics = _playerStatsController.CharacterStatistics;
            if (!characterStatistics) return;

            _image.fillAmount = Mathf.Clamp01(
                Mathf.InverseLerp(_min, characterStatistics.GetStats(Types.StatisticEnum.MaxHp), _playerHealth.Hp));
        }
    }
}