using Assets.Scripts.Controller.Collectible;
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

        public void OnGoldCollect(float value)
        {
            _value += (int) value;
        }
    }
}