using Assets.Scripts.Types;
using Assets.Scripts.Types.Settings.Upgrades;
using System;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Settings
{
    public class UpgradeSettings : ScriptableObject
    {
        [SerializeField]
        public UpgradeColorSettings Color;
        [SerializeField]
        public UpgradeRedrawCostSettings RedrawCost;
    }
}