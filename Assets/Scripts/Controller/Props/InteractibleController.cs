using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleController : MonoBehaviour
{
    Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _animator.SetTrigger("isOpen");
        }
    }

    public void TriggerAnimation()
    {

    }
}