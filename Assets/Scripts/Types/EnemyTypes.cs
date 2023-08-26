using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Types
{
    public enum EnemyBehavior
    {
        FollowPlayer,
        GatherCollectible
    }

    public enum EnemyDamage
    {
        Collision,
    }
    public enum EnemyDrop
    {
        Collecticle,
        None
    }
}