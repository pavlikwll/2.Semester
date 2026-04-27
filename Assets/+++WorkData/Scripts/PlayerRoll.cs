using System;
using UnityEngine;

    [RequireComponent(typeof(PlayerState))]
    public class PlayerRoll : MonoBehaviour
    {
        
        public static Action OnRollInput;
        public int actionId = 1;
        public float rollForce = 5f;
        private PlayerState _playerState;

        private void Awake()
        {
            _playerState = GetComponent<PlayerState>();
        }

        private void OnEnable()
        {
            OnRollInput += Roll;
        }

        private void OnDisable()
        {
            OnRollInput -= Roll;
        }

        void Roll()
        {
            if (_playerState.GetPlayerAction() != PlayerState.PlayerAction.Default) return;
            
            PlayerState.OnChangeAction?.Invoke(PlayerState.PlayerAction.Roll);
            Animations.OnAction?.Invoke(actionId);
            PlayerController.OnForceApply?.Invoke(rollForce);
        }
    }