using Assets.Scripts.Controller.Collectible.Soul;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Controller.Collectible
{
    public class KeyCollectible : CollectibleItem
    {
        public InteractibleController Interactible { get; set; }
        public Color Color { get; set; }
    }
}