using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.ScriptableObjects.Items;

public class GameEventListenerWeapon : GameEventListener<WeaponSO, GameEventWeapon, UnityEvent<WeaponSO>> { };