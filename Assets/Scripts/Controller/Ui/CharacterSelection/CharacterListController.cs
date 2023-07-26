using Assets.Scripts.Controller.Ui.CharacterSelection;
using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller.Ui
{
    public class CharacterListController : MonoBehaviour
    {
        GameObject _characterInfoPrefab;
        PlayableCharacterSO[] _playableCharacters;

        // Use this for initialization
        void Awake()
        {
            _characterInfoPrefab = Resources.Load<GameObject>("Prefabs/UI/CharacterInfo");
            _playableCharacters = Resources.LoadAll<PlayableCharacterSO>("ScriptableObjects/PlayableCharacters/");

            foreach(PlayableCharacterSO playbleCharacter in _playableCharacters)
            {
                GameObject characterInfo = Instantiate(_characterInfoPrefab, transform);
                CharacterInfoController characterInfoController = characterInfo.GetComponent<CharacterInfoController>();
                characterInfoController.Init(playbleCharacter);
            }
        }


    }
}