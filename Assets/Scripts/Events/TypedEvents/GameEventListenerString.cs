using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Events.TypedEvents
{
    public class GameEventListenerString : GameEventListener<string,GameEventString,UnityEvent<string>> {
    }
}