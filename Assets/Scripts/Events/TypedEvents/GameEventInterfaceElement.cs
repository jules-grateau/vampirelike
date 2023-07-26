using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Types;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName = "GameEvent with Interface Element Param", menuName = "GameEvent/InterfaceElement", order = 4)]
    public class GameEventInterfaceElement : GameEvent<InterfaceElement> { }
}
