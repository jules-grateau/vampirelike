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
    private float _speed = 6f;

    public bool forceCollect = false;


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
        var hit = Physics2D.OverlapCircle(transform.position, radius, 1 << LayerMask.NameToLayer("Player"));

        float distance = Vector2.Distance(_player.transform.position, transform.position);

        // Don't know why distance is some times bigger than radius even when giving radius to OverlapCircle
        if (!forceCollect && (!hit || distance > radius))
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        else
        {
            forceCollect = true;
        }

        var direction = _player.transform.position - transform.position;

        float flow = forceCollect ? Mathf.Max(distance * 0.5f, 1f) : 1 - (distance / radius) ;
        _rigidbody.velocity = direction.normalized * _speed * flow;
    }
}
