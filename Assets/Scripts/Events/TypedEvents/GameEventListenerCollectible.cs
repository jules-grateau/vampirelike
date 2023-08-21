using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.ScriptableObjects.Items;
using Assets.Scripts.Controller.Collectible;

namespace Assets.Scripts.Events.TypedEvents
{
    public class GameEventListenerCollectible : GameEventListener<CollectibleItem, GameEventCollectible, UnityEvent<CollectibleItem>> {
    }
}