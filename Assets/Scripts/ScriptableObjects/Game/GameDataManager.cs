using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Game
{
    [CreateAssetMenu(menuName = "GameDataManager")]
    public class GameDataManager : ScriptableObject
    {
        public class GameData : ScriptableObject
        {
            private static GameData _instance;

            public PlayableCharacterSO PlayableCharacter;
            public AnimationCurve XpCurve;
            public AnimationCurve DifficultyCurve;
            public float SpawnCooldown;

            public static GameData GetInstance(PlayableCharacterSO PlayableCharacter, AnimationCurve XpCurve, AnimationCurve DifficultyCurve, float SpawnCooldown)
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<GameData>();
                }
                if (!_instance)
                {
                    _instance = CreateInstance<GameData>();
                    _instance.PlayableCharacter = PlayableCharacter;
                    _instance.XpCurve = XpCurve;
                    _instance.DifficultyCurve = DifficultyCurve;
                    _instance.SpawnCooldown = SpawnCooldown;
                }
                return _instance;
            }
        }

        [SerializeField]
        public PlayableCharacterSO PlayableCharacter;

        [SerializeField]
        public AnimationCurve XpCurve;

        [SerializeField]
        public AnimationCurve DifficultyCurve;

        [SerializeField]
        public float SpawnCooldown;

        public GameData GetInstance()
        {
            return GameData.GetInstance(PlayableCharacter, XpCurve, DifficultyCurve, SpawnCooldown);
        }
    }
}