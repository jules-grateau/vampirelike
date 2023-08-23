using Assets.Scripts.Controller.Collectible;
using System;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerGold : MonoBehaviour
    {
        public int Value => _value;
        private int _value;

        public bool UseGold(int amount)
        {
            bool canUse = CanUse(amount);

            if(canUse) _value -= amount;

            return canUse;
        }

        public bool CanUse(int amount)
        {
            return amount <= _value;
        }

        public void OnGoldCollect(CollectibleItem collectible)
        {
            if (collectible is not ValueCollectible) return;
            ValueCollectible valueCollectible = (ValueCollectible)collectible;

            _value += (int) valueCollectible.Value;
        }
    }
}