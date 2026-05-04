using System;
using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerAttack : MonoBehaviour
{
    public static Action OnAttackInput;
    
    [SerializeField] private int ActionId = 2;
    private PlayerState _playerState;

    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
    }

    private void OnEnable()
    {
        OnAttackInput += Attack;
    }

    private void OnDisable()
    {
        OnAttackInput -= Attack;
    }

    private void Attack()
    {
        Debug.Log("Attack called");
        
        if (_playerState.GetPlayerAction() != PlayerState.PlayerAction.Default)
        {
            return;
        }
        
        PlayerState.OnChangeAction?.Invoke(PlayerState.PlayerAction.Attack);
        Animations.OnAction?.Invoke(ActionId);
    }
}