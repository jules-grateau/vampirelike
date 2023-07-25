using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.ScriptableObjects.Items;

namespace Assets.Scripts.Events.TypedEvents
{
    public class GameEventListenerCollectible : GameEventListener<CollectibleSO, GameEventCollectible, UnityEvent<CollectibleSO>> {
    }
}