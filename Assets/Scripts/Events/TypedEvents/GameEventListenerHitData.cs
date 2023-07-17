using UnityEngine.Events;

namespace Assets.Scripts.Events.TypedEvents
{
    public class GameEventListenerHitData : GameEventListener<HitData,GameEventHitData,UnityEvent<HitData>> { }
}