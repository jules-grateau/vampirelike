using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Types
{
    public enum ProjectileDirection
    {
        AutoAimed = 3,
        Straight = 2,
        Ricochet = 1,
        None = 0
    }
    public enum ProjectileBehaviourEnum
    {
        Normal,
        Grow
    }
    public enum ProjectileDamages
    {
        Direct,
        PerSecond,
        Explosion
    }

    public enum ProjectileDestruction
    {
        RandomAfterTime = 3,
        DestroyOnRangeReach = 2,
        DestroyNbrOfHits = 1,
        None = 0
    }
}