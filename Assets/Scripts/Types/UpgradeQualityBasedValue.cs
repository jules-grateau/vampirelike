using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Types
{
    [Serializable]
    public struct UpgradeQualityBasedValue<T>
    {
        public UpgradeQuality Quality;
        public T Value;
    }
}