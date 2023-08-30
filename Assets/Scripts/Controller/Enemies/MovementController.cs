using Assets;
using Assets.Scripts.Controller.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Types;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovementController : IEnemyMovement
{
    [SerializeField]
    protected GameObject _player;
    protected Rigidbody2D _rigidbody;
    protected EnemyHealth _enemyHealth;

    private bool _isFlipped = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    protected abstract Vector3 ComputeDirection();

    // Update is called once per frame
    void Update()
    {
        if (!_player) return;
        if (_enemyHealth.hasStun())
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        
        float modifiedSpeed = Speed * _enemyHealth.getSlowValue();

        Vector3 direction = ComputeDirection();
        if (_enemyHealth.hasFear())
        {
            direction = - (_player.transform.position - transform.position);
        }

        if (Mathf.Abs(direction.magnitude) < 0.1)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody.velocity = direction.normalized * modifiedSpeed;
        }
        bool needToFlip = direction.normalized.x < 0;

        if (needToFlip != _isFlipped)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            _isFlipped = needToFlip;
        }
    }
}
