using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Types
{
    public enum ProjectileDirection
    {
        AutoAimed,
        Straight,
        None
    }
    public enum ProjectileBehaviourEnum
    {
        Normal,
        Grow
    }
    public enum ProjectileDamages
    {
        Direct,
        PerSecond
    }

    public enum ProjectileDestruction
    {
        RandomAfterTime,
        DestroyOnRangeReach,
        None
    }
}