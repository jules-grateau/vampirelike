using Assets.Scripts.ScriptableObjects.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events.TypedEvents
{
    [CreateAssetMenu(fileName = "GameEvent with Collectible Param", menuName = "GameEvent/Collectible", order = 3)]
    public class GameEventCollectible : GameEvent<CollectibleSO> { }
}
