using Assets.Scripts.Variables;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameTimeController : MonoBehaviour
    {
        [SerializeField]
        FloatVariable _gameTime;

        TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_gameTime.value);

            if(timeSpan.TotalHours < 1)
            {
                _text.SetText(timeSpan.ToString(@"mm\:ss"));
                return;
            }

            _text.SetText(timeSpan.ToString(@"hh\:mm\:ss"));
        }

    }
}