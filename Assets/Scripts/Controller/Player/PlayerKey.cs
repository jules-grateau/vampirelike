using Assets.Scripts.Events;
using Assets.Scripts.Variables;
using Assets.Scripts.Variables.Constants;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controller.Game;
using UnityEngine;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Controller.Collectible;

namespace Assets.Scripts.Controller.Player
{
    public class PlayerKey : MonoBehaviour
    {
        private List<InteractibleController> _keyInteractibles;

        void Awake()
        {
            _keyInteractibles = new List<InteractibleController>();
        }

        private void Start()
        {

        }

        public bool canUnlock(InteractibleController interactible)
        {
            if (_keyInteractibles.Contains(interactible))
            {
                _keyInteractibles.Remove(interactible);
                return true;
            }
            return false;
        }

        public void OnPlayerCollectKey(CollectibleItem collectible)
        {
            KeyCollectible keyCollectible = (KeyCollectible)collectible;
            _keyInteractibles.Add(keyCollectible.Interactible);
        }
    }
}