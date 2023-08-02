using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    public abstract class UpgradeSO : ScriptableObject
    {
        public string Title => _title;
        [SerializeField]
        string _title;

        public Sprite Sprite => _sprite;
        [SerializeField]
        Sprite _sprite;

        public string Description => _description;
        [SerializeField]
        protected string _description;

        public int MaxAmount => _maxAmout;
        [SerializeField]
        int _maxAmout;

        public abstract string getDescription();
    }
}