using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.Controller.Ui
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LoadingTextController : MonoBehaviour
    {
        TextMeshProUGUI _textMP;
        [SerializeField]
        string _textToAdd;
        [SerializeField]
        float _delayBetweenCharacter;
        float _animationTime;
        int _lastAddedCharacter = -1;

        // Use this for initialization
        void Start()
        {
            _textMP = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            int character = _lastAddedCharacter + 1;

            if(character >= _textToAdd.Length)
            {
                _textMP.text = _textMP.text.Substring(0, _textMP.text.Length - character);
                _lastAddedCharacter = -1;
            } else
            {
                _textMP.text += _textToAdd[character];
                _lastAddedCharacter = character;
            }
        }
    }
}