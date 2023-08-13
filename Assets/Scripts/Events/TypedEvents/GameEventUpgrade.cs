using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Types;
using System;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName = "GameEvent with Upgrade Param", menuName = "GameEvent/Upgrade", order = 6)]
    public class GameEventUpgrade : GameEvent<Upgrade<UpgradeSO>> { }
}
