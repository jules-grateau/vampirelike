using System.Collections;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "PlayableCharacter", menuName = "PlayableCharacter", order = 1)]
    public class PlayableCharacterSO : ScriptableObject
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

        public GameObject Prefab => _prefab;
        [SerializeField]
        GameObject _prefab;
    }
}