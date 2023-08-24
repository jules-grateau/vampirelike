using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName ="GameEvent with WeaponSO param",menuName = "GameEvent/Weapon", order = 2)]
    public class GameEventWeapon : GameEvent<WeaponSO> { }
}