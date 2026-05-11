using System;
using Unity.VisualScripting;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private bool isMoving = false;
    
    private int _hashMovementValue = Animator.StringToHash("MovementValue");
    private int _hashActionId = Animator.StringToHash("ActionId");
    private int _hashActionTrigger = Animator.StringToHash("ActionTrigger");
    private int _hashDirXValue = Animator.StringToHash("xDir");
    private int _hashDirYValue = Animator.StringToHash("yDir");

    public static Action<int> OnAction;
    
    [Header("Animators")]
    [SerializeField] private Animator[] animators;
    
    [SerializeField] private Transform vision;
    
    
    #region Inspector

    [SerializeField] private Animator animator;

    #endregion

    #region Private Variables

    private Vector2 _moveInput;
    public Vector2 _lastMoveDirection = Vector2.down;
    private PlayerController _playerController;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    
    private void OnEnable()
    {
        OnAction += SetAnimationAction;
    }

    private void OnDisable()
    {
        OnAction -= SetAnimationAction;
    }

    #endregion

    #region Animation Methodes
    
    private void LateUpdate()
    {
        SetMovementAnimationValues();
        Flip();
    }
    
    void SetMovementAnimationValues()
    {
        if (_playerController.MoveInput.sqrMagnitude > 0.01f)
        {
            _lastMoveDirection = _playerController.MoveInput.normalized;
        }
        
        animator.SetFloat(_hashMovementValue, Mathf.Abs(_playerController.Rb.linearVelocity.magnitude));
        //animator.SetFloat(_hashDirXValue, _playerController.MoveInput.x);
        //animator.SetFloat(_hashDirYValue, _playerController.MoveInput.y);
        animator.SetFloat(_hashDirXValue, _lastMoveDirection.x);
        animator.SetFloat(_hashDirYValue, _lastMoveDirection.y);
        
        float movementValue = _playerController.Rb.linearVelocity.magnitude;
        //float xDir = _playerController.MoveInput.x;
        //float yDir = _playerController.MoveInput.y;
        float xDir = _lastMoveDirection.x;
        float yDir = _lastMoveDirection.y;
        
        foreach (Animator animator in animators)
        {
            animator.SetFloat(_hashMovementValue, movementValue);
            animator.SetFloat(_hashDirXValue, xDir);
            animator.SetFloat(_hashDirYValue, yDir);
        }
    }

    void SetAnimationAction(int ActionId)
    {
        foreach (Animator animator in animators)
        {
            animator.SetInteger(_hashActionId, ActionId);
            animator.SetTrigger(_hashActionTrigger);
        }
    }

    private void UpdateAnimations(Vector2 moveInout)
    {
        _moveInput = moveInout;

        if (isMoving)
        {
        }
        
    }
    
    private void Flip()
    {
        if (_playerController.MoveInput.sqrMagnitude > 0.01f)
        {
            _lastMoveDirection = _playerController.MoveInput.normalized;

            if (_playerController.MoveInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (_playerController.MoveInput.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    #endregion
}