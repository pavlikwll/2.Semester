using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region InputActions

    private InputSystem_Actions _inputActions;
    private InputAction _moveAction;
    private InputAction _rollAction;
    private InputAction _attackAction;
    private InputAction _interactAction;

    #endregion
    
    private Vector2 _lastMoveInput;

    #region Unity Events

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _moveAction = _inputActions.Player.Move;
        _rollAction = _inputActions.Player.Roll;
        _attackAction = _inputActions.Player.Attack;
        _interactAction = _inputActions.Player.Interact;
    }

    private void OnEnable()
    {
        EnableInput();
        
        _inputActions.Enable();
        _moveAction.performed += Move;
        _moveAction.canceled += Move;
        _rollAction.performed += Roll;
        _attackAction.performed += Attack;
        _interactAction.performed += Interact;
    }

    private void OnDisable()
    {
        DisableInput();
        
        _inputActions.Disable();
        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;
        _rollAction.performed -= Roll;
        _attackAction.performed -= Attack;
        _interactAction.performed -= Interact;
  
    }

    #endregion

    #region Input Methods

    public void EnableInput()
    {
        _inputActions.Enable();
    }
    
    public void DisableInput()
    {
        _inputActions.Disable();
    }
    // Метод руху
    // ctx містить інформацію про натиснуту кнопку
    private void Move(InputAction.CallbackContext ctx)
    {
        // Зчитуємо напрямок (наприклад: (1,0) - вправо)
        Vector2 newInput = ctx.ReadValue<Vector2>();
        
        // Якщо напрямок змінився
        if (_lastMoveInput != newInput)
        {
            // Рахуємо, що змінилося більше: X чи Y
            float xValue = Mathf.Abs(_lastMoveInput.x - newInput.x);
            float yValue = Mathf.Abs(_lastMoveInput.y - newInput.y);

            // Це потрібно, щоб визначити напрямок персонажа (вліво/вправо/вгору/вниз)
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
            
        // Передаємо рух у PlayerController
        // саме він рухає Rigidbody
        PlayerController.OnMoveInput?.Invoke(ctx.ReadValue<Vector2>());
        // Запам’ятовуємо новий напрямок
        _lastMoveInput = ctx.ReadValue<Vector2>();
    }
    
    // Метод перекату
    private void Roll(InputAction.CallbackContext ctx)
    {
        // Викликаємо подію roll
        // далі її підхоплює PlayerRoll
        PlayerRoll.OnRollInput?.Invoke();
    }
    
    private void Attack(InputAction.CallbackContext ctx)
    {
        PlayerAttack.OnAttackInput?.Invoke();
    }
    
    private void Interact(InputAction.CallbackContext ctx)
    {
        PlayerInteraction.OnInteract?.Invoke();
    }
    
    #endregion
}