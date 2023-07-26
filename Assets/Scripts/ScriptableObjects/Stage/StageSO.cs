using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Stage
{
    [CreateAssetMenu(fileName = "Stage", menuName = "Stage", order = 1)]
    public class StageSO : ScriptableObject
    {
        public string Name => _name;
        [SerializeField]
        string _name;

        public Sprite Sprite => _sprite;
        [SerializeField]
        Sprite _sprite;

        public string Description => _description;
        [SerializeField]
        string _description;

        public string SceneName => _sceneName;
        [SerializeField]
        string _sceneName;
    }
}