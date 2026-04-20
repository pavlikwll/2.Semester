using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action<Vector2> OnMoveInput;
    #region Inspector

    [Header("Movement Settings")] 
    [SerializeField] private float walkingSpeed = 5f;
    [SerializeField] private float accelerationTime = 10f;
    

    #endregion

    #region Private Variables

    public Rigidbody2D _rb;
    public Vector2 _moveInput;
    private float _currentSpeed;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentSpeed = walkingSpeed;
    }

    private void OnEnable()
    {
        OnMoveInput += SetMoveInput;
    }

    private void FixedUpdate()
    {
        MoveHandler();
    }

    private void OnDisable()
    {
        OnMoveInput -= SetMoveInput;
    }

    #endregion

    #region Handler Methods

    void MoveHandler()
    {
        Vector2 targetVelocity = _moveInput * _currentSpeed;
        Vector2 currentVelocity = _rb.linearVelocity;

        _rb.linearVelocity = Vector2.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * accelerationTime);
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }
   
    #endregion
}