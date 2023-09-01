using Assets.Scripts.Controller.Game;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Assets.Scripts.Controller.Ui
{
    public class DisplayTotalEnemyKilled : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _text;
        [SerializeField]
        LocalizedString _beforeValueText;
        [SerializeField]
        string _betweenValueAndText = "";

        [SerializeField]
        bool _refreshOnUpdate = true;

        // Use this for initialization
        void Start()
        {
            if (!_text) _text = GetComponentInChildren<TextMeshProUGUI>();

            UpdateText();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_refreshOnUpdate) return;
            UpdateText();

        }

        void UpdateText()
        {
            string text = "";
            if(!_beforeValueText.IsEmpty)
            {
                text += _beforeValueText.GetLocalizedString();
                text += _betweenValueAndText;
            }
            text += GameStatistics.Instance.EnemiesKilled.ToString();

            _text.SetText(text);
        }
    }
}