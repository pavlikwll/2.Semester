using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region InputActions

    private InputSystem_Actions _inputActions;
    private InputAction _moveAction;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _moveAction = _inputActions.Player.Move;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _moveAction.performed += Move;
        _moveAction.canceled += Move;
        
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;
    }

    #endregion

    #region Input Methods

    private void Move(InputAction.CallbackContext ctx)
    {
        //_moveInput = ctx.ReadValue<Vector2>();
        PlayerController.OnMoveInput(ctx.ReadValue<Vector2>());
    }
    
    #endregion
}