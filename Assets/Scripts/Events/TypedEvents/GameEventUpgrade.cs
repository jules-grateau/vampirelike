using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName = "GameEvent with Upgrade Param", menuName = "GameEvent/Upgrade", order = 6)]
    public class GameEventUpgrade : GameEvent<UpgradeSO> { }
}
