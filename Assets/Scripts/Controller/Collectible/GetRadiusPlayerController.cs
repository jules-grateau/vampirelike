using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller.Player;

[RequireComponent(typeof(Rigidbody2D))]
public class GetRadiusPlayerController : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private float _speed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player) return;

        float radius = _player.GetComponent<PlayerCollect>().getRadius();
        float distance = Vector2.Distance(_player.transform.position, transform.position);
        if (distance <= radius)
        {
            var direction = _player.transform.position - transform.position;
            _rigidbody.velocity = direction.normalized * _speed * ( 1 - (distance / radius)) * 2;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
