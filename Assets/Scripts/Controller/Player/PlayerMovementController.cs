using UnityEngine;
using Assets.Scripts.ScriptableObjects.Characters;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _lookRight = true;
    private Animator _animator;
    private PlayerStatsController _playerStatsController;

    [SerializeField]
    private float _defaultSpeed = 2.5f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerStatsController = GetComponent<PlayerStatsController>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Rotate(horizontalInput);
        Animate(horizontalInput, verticalInput);

        float speed = _defaultSpeed * (1 + getBonusSpeed()/100);

        _rigidbody.velocity = new Vector2(horizontalInput, verticalInput).normalized * speed;
    }

    private float getBonusSpeed()
    {
        CharacterStatisticsSO characterStatistics = _playerStatsController.CharacterStatistics;
        if (!characterStatistics) return 0f;
        return characterStatistics.GetStats(Assets.Scripts.Types.CharacterStatisticEnum.MovementSpeed);
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
