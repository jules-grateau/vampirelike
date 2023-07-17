using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName = "GameEvent with float param", menuName = "GameEvent/HitData", order = 2)]
    public class GameEventHitData : GameEvent<HitData> { }
}