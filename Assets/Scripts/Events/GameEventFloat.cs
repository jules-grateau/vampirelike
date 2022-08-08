using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events
{
    [CreateAssetMenu(fileName ="GameEvent with float param",menuName = "GameEvent/Float Param", order = 2)]
    public class GameEventFloat : GameEvent<float> { }
}