using Assets;
using Assets.Scripts.Controller.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Types;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayerController : IEnemyMovement
{
    [SerializeField]
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    private EnemyHealth _enemyHealth;

    private bool _isFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player) return;
        if (_enemyHealth.hasImpairment()) return;

        var direction = _player.transform.position - transform.position;
        _rigidbody.velocity = direction.normalized * Speed;

        bool needToFlip = direction.normalized.x < 0;

        if (needToFlip != _isFlipped)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _isFlipped = needToFlip;
        }
    }
}
