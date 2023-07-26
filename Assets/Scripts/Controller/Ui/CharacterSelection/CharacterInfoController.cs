using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui.CharacterSelection
{
    public class CharacterInfoController : MonoBehaviour
    {
        TextMeshProUGUI _name;
        Image _image;
        TextMeshProUGUI _description;

        [SerializeField]
        GameDataSO _gameData;

        PlayableCharacterSO _playableCharacter;

        public void Init(PlayableCharacterSO playableCharacter)
        {
            _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            _image = transform.Find("Image").GetComponent<Image>();
            _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            _playableCharacter = playableCharacter;

            _name.text = playableCharacter.Name;
            _image.sprite = playableCharacter.Sprite;
            _description.text = playableCharacter.Description;
        }

        public void OnClick()
        {
            _gameData.PlayableCharacter = _playableCharacter;
        }

    }
}