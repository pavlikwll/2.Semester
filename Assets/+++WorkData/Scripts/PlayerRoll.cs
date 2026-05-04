using System;
using UnityEngine;

    [RequireComponent(typeof(PlayerState))]
    public class PlayerRoll : MonoBehaviour
    {
        
        public static Action OnRollInput;
        public int ActionId = 1;
        public float rollForce = 12f;
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
            Debug.Log("Roll called");
            if (_playerState.GetPlayerAction() != PlayerAction.Default)
            {
                return;
            }
            
            PlayerState.OnChangeAction?.Invoke(PlayerAction.Roll);
            Animations.OnAction?.Invoke(ActionId);
            PlayerController.OnForceApply?.Invoke(rollForce);
        }
    }