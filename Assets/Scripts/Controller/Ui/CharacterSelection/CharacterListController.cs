using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.ScriptableObjects.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controller.Ui
{
    public class CharacterListController : MonoBehaviour
    {
        GameObject _characterInfoPrefab;
        PlayableCharacterSO[] _playableCharacters;
        List<GameObject> _characterInfo;

        // Use this for initialization
        void Awake()
        {
            _characterInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/CharacterInfo");
            _playableCharacters = Resources.LoadAll<PlayableCharacterSO>("ScriptableObjects/PlayableCharacters/");
            _characterInfo = new List<GameObject>();

            for (int i = 0; i < _playableCharacters.Length; i++)
            {
                PlayableCharacterSO playbleCharacter = _playableCharacters[i];
                GameObject characterInfo = Instantiate(_characterInfoPrefab, transform);
                CharacterInfoController characterInfoController = characterInfo.GetComponent<CharacterInfoController>();
                characterInfoController.Init(playbleCharacter);
                _characterInfo.Add(characterInfo);
            }
        }

        private void OnEnable()
        {
            if (_characterInfo.Count <= 0) return;

            EventSystem.current.SetSelectedGameObject(_characterInfo[0]);
        }


    }
}