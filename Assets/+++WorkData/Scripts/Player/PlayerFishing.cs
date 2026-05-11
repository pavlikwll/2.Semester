using System;
using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerFishing : MonoBehaviour
{
    public static Action OnFishingInput;
    [SerializeField] private int actionId = 3;
    private PlayerState _playerState;
    [SerializeField] private Animator[] animators;

    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
    }

    private void OnEnable()
    {
        OnFishingInput += Fishing;
    }

    private void OnDisable()
    {
        OnFishingInput -= Fishing;
    }

    public void Fishing()
    {
        if (_playerState.GetPlayerAction() != PlayerAction.Default)
        {
            Strike();
            return;
        }

        PlayerState.OnChangeAction?.Invoke(PlayerAction.Fishing);
        Animations.OnAction?.Invoke(actionId);
    }
    
    public void Strike()
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("FishingStrike");
        }
    }
}