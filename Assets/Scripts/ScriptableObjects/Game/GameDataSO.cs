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

        public AnimationCurve XpCurve
        {
            get { return _xpCurve; }
            set { _xpCurve = value; }
        }

        [SerializeField]
        AnimationCurve _xpCurve;

        public AnimationCurve DifficultyCurve
        {
            get { return _difficultyCurve; }
            set { _difficultyCurve = value; }
        }

        [SerializeField]
        AnimationCurve _difficultyCurve;

        public float SpawnCooldown
        {
            get { return _spawnCooldown; }
            set { _spawnCooldown = value; }
        }

        [SerializeField]
        float _spawnCooldown;
    }
}