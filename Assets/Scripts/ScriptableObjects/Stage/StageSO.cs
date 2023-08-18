using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace Assets.Scripts.ScriptableObjects.Stage
{
    [CreateAssetMenu(fileName = "Stage", menuName = "Stage", order = 1)]
    public class StageSO : ScriptableObject
    {
        public string Name => _name.GetLocalizedString();
        [SerializeField]
        LocalizedString _name;

        public Sprite Sprite => _sprite;
        [SerializeField]
        Sprite _sprite;

        public string Description => _description.GetLocalizedString();
        [SerializeField]
        LocalizedString _description;

        public string SceneName => _sceneName;
        [SerializeField]
        string _sceneName;
    }
}