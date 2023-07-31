using Assets.Scripts.ScriptableObjects.Game;
using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Ui.CharacterSelection
{
    public class CharacterInfoController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        TextMeshProUGUI _name;
        Image _image;
        TextMeshProUGUI _description;
        GameObject _statsInfo;

        [SerializeField]
        GameDataSO _gameData;

        PlayableCharacterSO _playableCharacter;

        public void Init(PlayableCharacterSO playableCharacter)
        {
            _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            _image = transform.Find("Image").GetComponent<Image>();
            _description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            _statsInfo = transform.Find("StatsInfo").gameObject;
            GameObject statInfoList = _statsInfo.transform.Find("List").gameObject;
            _playableCharacter = playableCharacter;

            _name.text = playableCharacter.Name;
            _image.sprite = playableCharacter.Sprite;
            _description.text = playableCharacter.Description;

            StatListController statsListController= statInfoList.AddComponent<StatListController>();
            playableCharacter.Init();
            playableCharacter.WeaponStatistics.Init();
            statsListController.Init(playableCharacter, playableCharacter.WeaponStatistics);
        }

        public void OnClick()
        {
            _gameData.PlayableCharacter = _playableCharacter;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _statsInfo.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _statsInfo.SetActive(false);
        }
    }
}