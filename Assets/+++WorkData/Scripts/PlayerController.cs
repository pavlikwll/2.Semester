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
    public Rigidbody2D _rb;
    public Rigidbody2D Rb => _rb;
    public Vector2 _moveInput;
    public Vector2 MoveInput => _moveInput;
    private float _currentSpeed;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerState = GetComponent<PlayerState>();
            
        _currentSpeed = walkingSpeed;
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
        if (_playerState.GetPlayerAction() == PlayerState.PlayerAction.Roll) return;
            
        Vector2 targetVelocity = _moveInput * _currentSpeed;
        Vector2 currentVelocity = _rb.linearVelocity;

        _rb.linearVelocity = Vector2.Lerp(
            currentVelocity, targetVelocity, Time.fixedTime * acceleration);
    }


    public void SetMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }
   
    #endregion
    
    #region Physics

    void ApplyForce(float force)
    {
        _rb.AddForce(_moveInput * force, ForceMode2D.Impulse);
    }

    #endregion
}