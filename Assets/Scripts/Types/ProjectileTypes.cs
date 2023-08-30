using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Types
{
    public enum ProjectileDirection
    {
        TurnAroundSpawnPosition=5,
        TurnBackTowardPlayer=4,
        AutoAimed = 3,
        Straight = 2,
        Ricochet = 1,
        None = 0
    }
    public enum ProjectileBehaviourEnum
    {
        Normal,
        Grow,
    }
    public enum ProjectileDamages
    {
        Direct,
        Explosion,
        Split,
    }

    public enum ProjectileDestruction
    {
        ReachPlayer = 4,
        RandomAfterTime = 3,
        OnRangeReach = 2,
        NbrOfHits = 1,
        None = 0
    }
}