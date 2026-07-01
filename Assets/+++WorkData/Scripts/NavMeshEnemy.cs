using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState{Idle, Patrolling, Chasing, Attacking, Dead}
public enum FacingDirection{Up, Down, Left, Right}

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshEnemy : MonoBehaviour
{
    private int HashMovementValue = Animator.StringToHash("MovementValue");
    private int HashDirX = Animator.StringToHash("dirX");
    private int HashDirY = Animator.StringToHash("dirY");
    private int HashActionTrigger = Animator.StringToHash("ActionTrigger");
    private int HashActionId = Animator.StringToHash("ActionId");
    
    #region Inspector

    [Header("States")] 
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private FacingDirection facingDirection;
    public FacingDirection FacingDirection => facingDirection;
    
    [Header("NPC References")] 
    [SerializeField] private Animator animator;
    [SerializeField] private bool startDirectionIsRight = false;

    [Header("Attack Setting")] 
    [SerializeField] public float stopChasingTimer = 1f;
    [SerializeField] private float attackCooldown = 1f;
    
    [Header("Waypoints")] 
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private bool waitAtWaypoint = true;
    [SerializeField] private bool randomOrder;
    [SerializeField] private bool canPatrol = true;

    [SerializeField] private Vector2 waitDuration = new Vector2(1, 5);
    

    [Header("Gizmos")] 
    [SerializeField] private bool showWaypoints = true;
    #endregion
    
    #region Private Variables

    private NavMeshAgent _agent;
    private Transform _player;
    [SerializeField] private Transform _target;

    private int _currentWaypointIndex = -1;
    private bool _isWaiting;

    public bool _isAggroed = false;
    public bool _isAttacking = false;
    public bool _isInAttackArea = false;
    public bool _canAttack = false;
    public bool _isChasing = false;

    private float _attackCooldownTimer;
    private Coroutine _attackCoroutine;
    private Coroutine _aggroCoroutine;
    #endregion
    
    #region Unity Event Functions

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = FindFirstObjectByType<PlayerController>().transform;
        _agent.autoBraking = waitAtWaypoint;
    }

    private void Start()
    {
        print(gameObject.name);
        print(animator.gameObject.name);
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        SetNextWaypoint();
    }

    private void Update()
    {
        /*
        if (_target != null && enemyState != EnemyState.Idle)
        {
            SetAnimationDirection(_agent.velocity);
            RotateObj(_agent.velocity);
        }
        */
        
        if (_canAttack && !_isAttacking)
        {
            _attackCooldownTimer += Time.deltaTime;
            
            if (_attackCooldownTimer > attackCooldown)
            {
                _isAttacking = true;
                enemyState = EnemyState.Attacking;
                //_attackCooldownTimer = 0;
                //TODO : Enemy Aniamtor Attack
            }
        }

        if (!_agent.isStopped && enemyState != EnemyState.Chasing && enemyState != EnemyState.Attacking)
        {
            CheckIfWaypointIsReached();
        }
        else if (!_agent.isStopped && enemyState == EnemyState.Chasing)
        {
            float distance = Vector2.Distance(transform.position, _player.position);

            if (distance > _agent.stoppingDistance + 0.05f)
            {
                _agent.SetDestination(_player.position);
            }
            else
            {
                _agent.ResetPath();
            }
        }
    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    #endregion

    #region Animation

    private void UpdateAnimator()
    {
        Vector2 velocity = _agent.desiredVelocity;

        float movementValue = velocity.sqrMagnitude > 0.01f ? 1f : 0f;

        animator.SetFloat(HashMovementValue, movementValue);

        if (movementValue > 0)
        {
            animator.SetFloat(HashDirX, velocity.normalized.x);
            animator.SetFloat(HashDirY, velocity.normalized.y);
            RotateObj(velocity.normalized);
        }
    }

    private void SetAnimationDirection(Vector2 direction)
    {
        animator.SetFloat(HashDirX, direction.x);
        animator.SetFloat(HashDirY, direction.y);
    }
    
    #endregion
    
    #region Navigation

    private void RotateObj(Vector2 direction)
    {
        if (direction.x < 0)
        {
            animator.transform.rotation = Quaternion.Euler(0, startDirectionIsRight ? 180 : 0, 0);
        }
        else if (direction.x > 0)
        {
            animator.transform.rotation = Quaternion.Euler(0, startDirectionIsRight ? 0 : 180, 0);
        }
    }

    private void UpdateFacingDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            facingDirection = dir.x > 0 ? FacingDirection.Right : FacingDirection.Left;
        }
        else
        {
            facingDirection = dir.y > 0 ? FacingDirection.Up : FacingDirection.Down;
        }

        switch (facingDirection)
        {
            case FacingDirection.Up:
                SetAnimationDirection(new Vector2(0, 1));
                break;
            
            case FacingDirection.Down:
                SetAnimationDirection(new Vector2(0, -1));
                break;
            
            case FacingDirection.Left:
                SetAnimationDirection(new Vector2(-1, 0));
                break;
            
            case FacingDirection.Right:
                SetAnimationDirection(new Vector2(1, 0));
                break;
        }
    }
    

    private void StopPatrolForDialogue()
    {
        //TODO: Dialogue Logic
    }
    
    public void StopPatrol()
    {
        _agent.isStopped = true;
    }

    public void ResumePatrol()
    {
        _agent.isStopped = false;
    }
    
    public void TogglePatrol()
    {
        _agent.isStopped = !_agent.isStopped;
        canPatrol = !canPatrol;
    }

    public void SetNewTarget(Transform newTarget)
    {
        _target = newTarget;
        _agent.isStopped = false;
        canPatrol = false;
        _agent.SetDestination(_target.position);
    }

    private void SetNextWaypoint()
    {
        enemyState = EnemyState.Patrolling;
        
        if (randomOrder)
        {
            int newWaypointIndex;

            do
            {
                newWaypointIndex = Random.Range(0, waypoints.Count);
            } while (newWaypointIndex == _currentWaypointIndex);

            _currentWaypointIndex = newWaypointIndex;
        }
        else
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count;
        }
        
        _target = waypoints[_currentWaypointIndex];
        
        _agent.SetDestination(waypoints[_currentWaypointIndex].position);
    }

    private void CheckIfWaypointIsReached()
    {
        if (_isWaiting) return;
        if (_agent.pathPending) return;

        if (_agent.remainingDistance <= _agent.stoppingDistance + 0.01f)
        {
            if (waitAtWaypoint)
            {
                StartCoroutine(WaitBeforeNextWaypoint(Random.Range(waitDuration.x, waitDuration.y)));
            }
            else
            {
                SetNextWaypoint();
            }
        }
    }

    private IEnumerator WaitBeforeNextWaypoint(float duration)
    {
        enemyState = EnemyState.Idle;
        _isWaiting = true;
        yield return new WaitForSeconds(duration);
        _isWaiting = false;
        SetNextWaypoint();

        enemyState = EnemyState.Patrolling;
    }

    public void SetNewWaypoints(List<Transform> newWaypoints)
    {
        waypoints = newWaypoints;
        canPatrol = true;
    }

    #endregion
    
    #region Aggro

    public void EnterAggroDistance()
    {
        _isAggroed = true;
        enemyState = EnemyState.Chasing;
        _target = _player;
        
        if(_aggroCoroutine != null)
            StopCoroutine(_aggroCoroutine);
    }

    public void ExitAggro()
    {
        _isAggroed = false;
        enemyState = EnemyState.Chasing;

        _aggroCoroutine = StartCoroutine(InitiateAggroTimer());
    }

    IEnumerator InitiateAggroTimer()
    {
        _isChasing = true;
        yield return new WaitForSeconds(stopChasingTimer);

        if (!_isAggroed)
        {
            enemyState = EnemyState.Idle;
            _isChasing = false;
            SetNextWaypoint();
        }
    }
    
    #endregion

    #region Attack

    public void EnterAttackDistance()
    {
        _canAttack = true;
        _agent.isStopped = true;
    }

    public void ExitAttackDistance()
    {
        _canAttack = false;
        enemyState = EnemyState.Chasing;
        _agent.isStopped = false;
    }

    public void EndAttack()
    {
        _isAttacking = false;
        enemyState = EnemyState.Chasing;
        _agent.isStopped = false;
    }

    #endregion
}