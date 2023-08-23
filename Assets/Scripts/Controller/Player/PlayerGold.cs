using Assets.Scripts.Controller.Collectible;
using System;
using UnityEngine;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerGold : MonoBehaviour
    {
        public int Value => _value;
        private int _value;

        public bool UseGold(int amount)
        {
            if (amount > _value) return false;

            _value -= amount;
            return true;
        }

        public void OnGoldCollect(CollectibleItem collectible)
        {
            if (collectible is not ValueCollectible) return;
            ValueCollectible valueCollectible = (ValueCollectible)collectible;

            _value += (int) valueCollectible.Value;
        }
    }
}