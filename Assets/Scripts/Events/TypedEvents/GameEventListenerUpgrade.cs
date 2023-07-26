using Assets.Scripts.ScriptableObjects;
using UnityEngine.Events;

namespace Assets.Scripts.Events.TypedEvents
{
    public class GameEventListenerUpgrade : GameEventListener<UpgradeSO, GameEventUpgrade, UnityEvent<UpgradeSO>> {
    }
}