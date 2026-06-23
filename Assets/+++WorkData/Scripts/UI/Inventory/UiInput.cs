using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiInput : MonoBehaviour
{
    #region InputActions
    private InputSystem_Actions _inputActions;
    private InputAction _escAction;
    private InputAction _inventoryAction;
    #endregion
    
    #region Unity Events

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _escAction = _inputActions.UI.Cancel;
        _inventoryAction = _inputActions.UI.Inventory;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _escAction.performed += Esc;
        _inventoryAction.performed += Inventory;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _escAction.performed -= Esc;
        _inventoryAction.performed -= Inventory;
    }
    #endregion
    
    #region Input Methods
    
    private void Esc(InputAction.CallbackContext ctx)
    {

    }
    
    private void Inventory(InputAction.CallbackContext ctx)
    {
        InventorySystem.Instance.ToggleInventory();
    }
    
    #endregion

}
