using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Types
{
    public class Upgrade<T> where T : UpgradeSO 
    {

        public Upgrade(UpgradeQuality quality, T upgradeSO)
        {
            UpgradeQuality = quality;
            UpgradeSO = upgradeSO;
        }

        public UpgradeQuality UpgradeQuality { get; }
        public T UpgradeSO { get; }

        public string GetDescription()
        {
            return UpgradeSO.GetDescription(UpgradeQuality);
        }


    }
}