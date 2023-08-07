using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Types
{
    [Serializable]
    public struct RarityBasedValue<T>
    {
        public UpgradeQuality Quality;
        public T Value;
    }
}