using UnityEngine;
using Assets.Scripts.ScriptableObjects.Characters;
using static UnityEngine.InputSystem.InputAction;
using Assets.Scripts.Types;
using Assets.Types;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _lookRight = true;
    private Animator _animator;
    private PlayerStatsController _playerStatsController;
    private PlayerInputs _inputActions;

    private BaseStatistics<CharacterStatisticEnum> _characterStatistics;
    public float Speed = 2.5f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _playerStatsController = GetComponent<PlayerStatsController>();

        _characterStatistics = _playerStatsController.CharacterStatistics;
        if (_characterStatistics == null) return;

        Speed = _characterStatistics.GetStats(Assets.Scripts.Types.CharacterStatisticEnum.MovementSpeed);
    }

    private void OnEnable()
    {
        _inputActions = InputManager.GetInstance();
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 movementValue = _inputActions.Gameplay.Movement.ReadValue<Vector2>();
        Rotate(movementValue.x);
        Animate(movementValue.x, movementValue.y);

        _rigidbody.velocity = new Vector2(movementValue.x, movementValue.y).normalized * Speed;
        _animator.speed = Speed / 2;
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
