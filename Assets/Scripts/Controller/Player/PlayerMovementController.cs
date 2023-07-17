using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _lookRight = true;
    private Animator _animator;

    [SerializeField]
    private float _speed = 10f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Rotate(horizontalInput);
        Animate(horizontalInput, verticalInput);

        _rigidbody.velocity = new Vector2(horizontalInput, verticalInput).normalized * _speed;



    }

    void Rotate(float horizontalInput)
    {
        if (horizontalInput < 0 && _lookRight)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            _lookRight = false;
        }

        if (horizontalInput > 0 && !_lookRight)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            _lookRight = true;
        }
    }

    void Animate(float horizontalInput, float verticalInput)
    {
        if (!_animator) return;

        if(horizontalInput != 0 || verticalInput != 0)
        {
            _animator.SetTrigger("startWalking");
        } else
        {
            _animator.SetTrigger("stopWalking");
        }
    }
  
}
