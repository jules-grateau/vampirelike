using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayerController : MonoBehaviour
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
        var direction = _player.transform.position - transform.position;
        _rigidbody.velocity = direction.normalized * _speed;
    }
}
