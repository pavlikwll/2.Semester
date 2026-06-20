using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action<Vector2> OnMoveInput;
    public static Action<float> OnForceApply;
    
    #region Inspector

    [Header("Movement Settings")] 
    [SerializeField] private float walkingSpeed = 5f;
    [SerializeField] private float accelerationTime = 10f;
    
    #endregion

    #region Private Variables

    private PlayerState _playerState;
    private Rigidbody2D _rb;

    public Rigidbody2D Rb => _rb;
    public Vector2 MoveInput => _moveInput;
    public Vector2 LastMoveDirection => _lastMoveDirection;

    private Vector2 _moveInput;
    private Vector2 _lastMoveDirection = Vector2.down;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerState = GetComponent<PlayerState>();
            
        //_currentSpeed = walkingSpeed;
    }

    private void OnEnable()
    {
        OnMoveInput += SetMoveInput;
        OnForceApply += ApplyForce;
    }

    private void FixedUpdate()
    {
        MoveHandler();
    }

    private void OnDisable()
    {
        OnMoveInput -= SetMoveInput;
        OnForceApply -= ApplyForce;
    }

    #endregion

    #region Handler Methods

    void MoveHandler()
    {
        if (_playerState.GetPlayerAction() == PlayerAction.Roll|| _playerState.GetPlayerAction() == PlayerAction.Fishing)
        {
            return;
        }
        
        /*
         if (_playerState.GetPlayerAction() == PlayerState.PlayerAction.Roll || _playerState.GetPlayerAction() == PlayerState.PlayerAction.Attack)
         {
            return;
         }
         */
            
        Vector2 targetVelocity = _moveInput * walkingSpeed;
        Vector2 currentVelocity = _rb.linearVelocity;

        _rb.linearVelocity = Vector2.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * accelerationTime);
    }


    public void SetMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
        
        PlayerState.OnChangeMovement?.Invoke(_moveInput == Vector2.zero ? PlayerMovement.Idle : PlayerMovement.Moving);
        
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            _lastMoveDirection = _moveInput.normalized;
            PlayerState.OnChangeDirection?.Invoke(_lastMoveDirection);
        }
        /*
        if (_moveInput == Vector2.zero)
        {
            PlayerState.Instance.SetMovementState(MovementState.Idle);
        }
        else
        {
            PlayerState.Instance.SetMovementState(MovementState.Walking);
        }
        */
    }
   
    #endregion
    
    #region Physics

    void ApplyForce(float force)
    {
        Vector2 rollDirection;
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            rollDirection = _moveInput.normalized;
        }
        else
        {
            rollDirection = _lastMoveDirection;
        }
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(rollDirection * force, ForceMode2D.Impulse);
    }

    #endregion
}