//Volodymyr Pavlik
using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance;
    public static Action<Vector2> OnChangeDirection;
    public static Action<float> OnHorizontalChangeDirection;
    public static Action<float> OnVerticalChangeDirection;
    public static Action<PlayerAction> OnChangeAction;
    public static Action<PlayerMovement> OnChangeMovement;
    
    [SerializeField] public PlayerDirection playerDirection;
    [SerializeField] public PlayerAction playerAction;
    [SerializeField] public PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void OnEnable()
    {
        OnChangeDirection += SetDirection;
        OnChangeAction += SetAction;
        OnChangeMovement += SetMovement;
        OnHorizontalChangeDirection += SetHorizontalDirection;
        OnVerticalChangeDirection += SetVerticalDirection;
        OnChangeMovement += SetMovement;
    }

    private void OnDisable()
    {
        OnChangeDirection -= SetDirection;
        OnChangeAction -= SetAction;
        OnChangeMovement -= SetMovement;
        OnHorizontalChangeDirection -= SetHorizontalDirection;
        OnVerticalChangeDirection -= SetVerticalDirection;
        OnChangeMovement -= SetMovement;
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
    void SetDirection(Vector2 _moveInput)
    {
        if (_moveInput.x < 0)
        {
            playerDirection = PlayerDirection.Left;
        }
        else if (_moveInput.x > 0)
        {
            playerDirection = PlayerDirection.Right;
        }
        else if (_moveInput.y < 0)
        {
            playerDirection = PlayerDirection.Down;
        }
        else if (_moveInput.y > 0)
        {
            playerDirection = PlayerDirection.Up;
        }
    }

    void SetHorizontalDirection(float xInput)
    {
        SetDirection(new Vector2(xInput, 0));
    }
    
    void SetVerticalDirection(float yInput)
    {
        SetDirection(new Vector2(0, yInput));
    }
    
    void SetAction(PlayerAction _playerAction)
    {
        playerAction = _playerAction;
    }
    
    void SetMovement(PlayerMovement _playerMovement)
    {
        playerMovement = _playerMovement;
    }

    public PlayerDirection GetPlayerDirection()
    {
        return playerDirection;
    }
    
    public PlayerAction GetPlayerAction()
    {
        return playerAction;
    }
    
    public PlayerMovement GetPlayerMovement()
    {
        return playerMovement;
    }

}
