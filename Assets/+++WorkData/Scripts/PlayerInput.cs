using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region InputActions

    private InputSystem_Actions _inputActions;
    private InputAction _moveAction;
    private InputAction _rollAction;

    #endregion
    
    private Vector2 _lastMoveInput;

    #region Unity Events

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _moveAction = _inputActions.Player.Move;
        _rollAction = _inputActions.Player.Roll;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _moveAction.performed += Move;
        _moveAction.canceled += Move;
        _rollAction.performed += Roll;

    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;
        _rollAction.performed -= Roll;
    }

    #endregion

    #region Input Methods

    private void Move(InputAction.CallbackContext ctx)
    {
        Vector2 newInput = ctx.ReadValue<Vector2>();

        if (_lastMoveInput != newInput)
        {
            float xValue = Mathf.Abs(_lastMoveInput.x - newInput.x);
            float yValue = Mathf.Abs(_lastMoveInput.y - newInput.y);

            if (xValue > yValue)
            {
                PlayerState.OnHorizontalChangeDirection?.Invoke(newInput.x);
            }
            else if(xValue < yValue)
            {
                PlayerState.OnVerticalChangeDirection?.Invoke(newInput.y);
            }
            else
            {
                PlayerState.OnChangeDirection?.Invoke(newInput);
            }
        }
            
        PlayerController.OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());

        _lastMoveInput = ctx.ReadValue<Vector2>();
    }
        
    private void Roll(InputAction.CallbackContext ctx)
    {
        PlayerRoll.OnRollInput?.Invoke();
    }
    
    #endregion
}