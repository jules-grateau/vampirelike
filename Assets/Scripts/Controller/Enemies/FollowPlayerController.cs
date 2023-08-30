using Assets;
using Assets.Scripts.Controller.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Types;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayerController : MovementController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override Vector3 ComputeDirection()
    {
        Vector3 direction = _player.transform.position - transform.position;
        direction -= direction.normalized * 0.5f;
        return direction;
    }
}
