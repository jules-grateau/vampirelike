using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Controller.Ui
{
    public class LevelIndicator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Value to use as the current level")]
        private IntVariable _currentLevel;

        private TextMeshProUGUI _levelText;
        private void Start()
        {
            _currentLevel.value = 1;
            _levelText = gameObject.GetComponent<TextMeshProUGUI>();
        }
        private void Update()
        {
            _levelText.SetText(_currentLevel.value.ToString());
        }
    }
}