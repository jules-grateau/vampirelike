using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Types;
using System;
using UnityEngine.Events;

namespace Assets.Scripts.Events.TypedEvents
{
    public class GameEventListenerUpgrade : GameEventListener<Upgrade<UpgradeSO>, GameEventUpgrade, UnityEvent<Upgrade<UpgradeSO>>> {
    }
}