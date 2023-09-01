using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName ="GameEvent with string param",menuName = "GameEvent/String", order = 2)]
    public class GameEventString : GameEvent<string> { }
}