using Assets.Scripts.Events;
using Assets.Scripts.Events.TypedEvents;
using Assets.Scripts.Types;
using UnityEngine.Events;

public class GameEventListenerInterfaceElement : GameEventListener<InterfaceElement, bool, GameEventInterfaceElement, UnityEvent<InterfaceElement, bool>> { };