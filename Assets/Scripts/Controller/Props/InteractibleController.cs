using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controller.Collectible;
using Assets.Scripts.Controller.Player;
using Assets.Scripts.Events.TypedEvents;

public abstract class InteractibleController : MonoBehaviour
{
    protected Animator _animator;
    [SerializeField]
    protected bool _isLocked = false;
    [SerializeField]
    public GameEventCollectible OnUseKeyEvent;

    private GameObject _lockRef;
    protected GameObject _lock;
    private Animator _lockAnimator;
    private KeyCollectible _key;

    // Start is called before the first frame update
    void Awake()
    {
        _lockRef = Resources.Load<GameObject>("Prefabs/Particles/lock_1");
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        if (_isLocked && _key)
        {
            _lock = Instantiate(_lockRef, gameObject.transform);
            _lock.GetComponent<SpriteRenderer>().color = _key.Color;
            _lockAnimator = _lock.GetComponent<Animator>();
        }
    }

    public void SetKey(KeyCollectible key)
    {
        _key = key;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_isLocked && _lockAnimator)
            {
                PlayerKey playerKey = collision.gameObject.GetComponent<PlayerKey>();
                if (playerKey && playerKey.canUnlock(this))
                {
                    OnUseKeyEvent.Raise(_key);
                    _lockAnimator.SetTrigger("open");
                    _isLocked = false;
                }
                else
                {
                    _lockAnimator.SetTrigger("no_open");
                }
            }
            else
            {
                _animator.SetTrigger("interract");
            }
        }
    }
    protected abstract void TriggerAnimation();
}