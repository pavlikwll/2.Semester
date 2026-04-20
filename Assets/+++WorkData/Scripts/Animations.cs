using System;
using Unity.VisualScripting;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private bool isMoving = false;
    
    private int _hashMovementValue = Animator.StringToHash("MovementValue");
    private int _hashDirXValue = Animator.StringToHash("xDir");
    private int _hashDirYValue = Animator.StringToHash("yDir");
    
    #region Inspector

    [SerializeField] private Animator animator;

    #endregion

    #region Private Variables

    private Vector2 _moveInput;
    private Vector2 _lastMoveDirection = Vector2.down;
    private PlayerController _playerController;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    #endregion

    #region Animation Methodes
    
    private void LateUpdate()
    {
        SetMovementAnimationValues();
    }
    
    void SetMovementAnimationValues()
    {
        animator.SetFloat(_hashMovementValue, Mathf.Abs(_playerController._rb.linearVelocity.magnitude));
        animator.SetFloat(_hashDirXValue, _playerController._moveInput.x);
        animator.SetFloat(_hashDirYValue, _playerController._moveInput.y);
    }

    private void UpdateAnimations(Vector2 moveInout)
    {
        _moveInput = moveInout;

        if (isMoving)
        {
        }
        
    }

    #endregion
}