using Assets.Scripts.Types;
using UnityEngine;
using UnityEngine.Localization;

namespace Assets.Scripts.ScriptableObjects
{
    public abstract class UpgradeSO : ScriptableObject
    {
        public string Title => _title.GetLocalizedString();
        [SerializeField]
        LocalizedString _title;

        public string Description => _description.GetLocalizedString();
        [SerializeField]
        protected LocalizedString _description;

        public int MaxAmount => _maxAmout;
        [SerializeField]
        int _maxAmout;

        public Sprite Sprite => _sprite;
        [SerializeField]
        Sprite _sprite;

        public abstract string GetDescription(UpgradeQuality quality);

        public abstract bool HasQuality(UpgradeQuality quality);
    }
}