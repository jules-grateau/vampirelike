using Assets.Scripts.Controller.Collectible.Soul;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Controller.Collectible
{
    public class XpCollectible : CollectibleItem
    {
        public float XpValue { get; set; }

        protected override void Collect()
        {
            base.Collect();
            OnCollectEvent.Raise(XpValue);
        }

        void Start()
        {
            SoulColorController soulColorController = GetComponent<SoulColorController>();
            if (!soulColorController) return;

            soulColorController.Init(XpValue / 100);
        }
    }
}