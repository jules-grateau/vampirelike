using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Game
{
    public class GameDataSO : ScriptableObject
    {
        public PlayableCharacterSO PlayableCharacter {
            get { return _playableCharacter; }
            set { _playableCharacter = value; }
        }

        [SerializeField]
        PlayableCharacterSO _playableCharacter;
    }
}