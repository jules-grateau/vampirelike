using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Events.TypedEvents
{
    public class GameEventListenerWeapon : GameEventListener<WeaponSO,GameEventWeapon,UnityEvent<WeaponSO>> {
    }
}