using Assets.Scripts.Types;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Settings
{
    public class UpgradeQualityColorSettingsSO : ScriptableObject
    {
        [SerializeField]
        UpgradeColor[] _qualityColor;

        Dictionary<UpgradeQuality, Color> _qualityColorDictionnary;

        public Color GetColor(UpgradeQuality quality)
        {
            GenerateDictonnary();
            return _qualityColorDictionnary.GetValueOrDefault(quality);
        }

        void GenerateDictonnary()
        {
            if (_qualityColorDictionnary != null) return;

            _qualityColorDictionnary = new Dictionary<UpgradeQuality, Color>();

            foreach(UpgradeColor upgradeColor in _qualityColor)
            {
                _qualityColorDictionnary.Add(upgradeColor.Quality, upgradeColor.Color);
            }
        }
    }

    [Serializable]
    struct UpgradeColor
    {
        [SerializeField]
        public UpgradeQuality Quality;
        [SerializeField]
        public Color Color;
    }
}