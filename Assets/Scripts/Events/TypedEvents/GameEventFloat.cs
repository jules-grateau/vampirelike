using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName ="GameEvent with float param",menuName = "GameEvent/Float", order = 2)]
    public class GameEventFloat : GameEvent<float> { }
}