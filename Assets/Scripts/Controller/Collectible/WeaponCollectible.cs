using Assets.Scripts.Controller.Collectible.Soul;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Controller.Collectible
{
    public class WeaponCollectible : CollectibleItem
    {
        public WeaponSO Weapon;

        void Start()
        {
            GetComponent<SpriteRenderer>().sprite = Weapon.icon;
        }
    }
}