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
        if (_isLocked && _key)
        {
            InitLock();
        }
    }

    public void SetKey(KeyCollectible key)
    {
        InitLock();
        _key = key;
        _lock.GetComponent<SpriteRenderer>().color = _key.Color;
    }

    private void InitLock()
    {
        if (!_lock)
        {
            _lock = Instantiate(_lockRef, gameObject.transform);
            _lockAnimator = _lock.GetComponent<Animator>();
        }
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
                if (_animator)
                {
                    _animator.SetTrigger("interract");
                }
                else
                {
                    TriggerAnimation();
                }
            }
        }
    }
    protected abstract void TriggerAnimation();

    protected void SetIsUsable(bool isUsable)
    {
        SpriteRenderer spriteOldTarget = gameObject.GetComponent<SpriteRenderer>();
        spriteOldTarget.material.SetInt("_Unusable", isUsable ? 0 : 1);
    }
}