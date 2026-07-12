//Volodymyr Pavlik
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavMeshPatrol : MonoBehaviour
{
    private static readonly int HashMovementValue = Animator.StringToHash("MovementValue");
    private static readonly int HashDirX = Animator.StringToHash("dirX");
    private static readonly int HashDirY = Animator.StringToHash("dirY");
    private static readonly int HashActionTrigger = Animator.StringToHash("ActionTrigger");
    private static readonly int HashActionId = Animator.StringToHash("ActionId");
    private static readonly int HashDeathTrigger = Animator.StringToHash("DeathTrigger");
    
    
    #region Inspector
    [Header("Enemy States")] 
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private EnemyFacingDirection enemyFacingDirection;

    [Header("Navigation")] 
    [SerializeField] private float navmeshPathTimer = .25f;
    
    [Header("NPC Reference")] 
    [SerializeField] private Animator animator;
    [SerializeField] private bool startDirectionIsRight = false;

    [Header("AttackSetting")] 
    [SerializeField] private float stopChasingTimer = 2f;

    [SerializeField] private float attackCooldown = 1f;
    
    [Header("Waypoints")] 
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private bool waitAtWaypoint = true;
    [SerializeField] private bool randomOrder;
    [SerializeField] private bool canPatrol = true;
    [SerializeField] private Vector2 waitDuration = new Vector2(1, 5);

    #endregion
    
    #region Private Variables

    private NavMeshAgent _agent;
    private Transform _target;
    private Transform _player;
    
    private int _currentWaypointIndex = -1;
    
    private bool _isWaiting;
    public bool _isAggroed = false;
    public bool _canAttack = false;

    public float _attackCooldownTimer;
    private float _lastNavmeshTime;

    private Coroutine _attackCoroutine;
    private Coroutine _aggroCoroutine;
    private Coroutine _newWaitpoint;

    private Vector2 _lookDirection;

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
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        
        if (waypoints != null && waypoints.Count > 0)
        {
            SetNextWaypoint();
        }
        else
        {
            enemyState = EnemyState.Idle;
        }
    }

    private void Update()
    {
        if (enemyState == EnemyState.Dead)
            return;

        if (_canAttack && enemyState != EnemyState.Attacking)
        {
            _attackCooldownTimer += Time.deltaTime;

            if (_attackCooldownTimer > attackCooldown)
            {
                enemyState = EnemyState.Attacking;
                SetAnimationAction(1);
            }
        }

        if (!_agent.isStopped &&
            enemyState != EnemyState.Chasing &&
            enemyState != EnemyState.Attacking)
        {
            CheckIfWaypointIsReached();
        }
        else if (!_agent.isStopped &&
                 enemyState == EnemyState.Chasing &&
                 _target != null)
        {
            float distance =
                Vector2.Distance(transform.position, _target.position);

            if (distance > _agent.stoppingDistance + 0.01f)
            {
                if (_lastNavmeshTime + navmeshPathTimer < Time.time)
                {
                    _agent.SetDestination(_target.position);
                    _lastNavmeshTime = Time.time;
                }
            }
            else
            {
                _agent.ResetPath();
            }
        }
    }


    private void LateUpdate()
    {
        if (enemyState == EnemyState.Dead)
            return;

        UpdateAnimator();
    }
    
    #endregion
    
    #region Navigation

    private void UpdateFacing()
    {
        Vector2 velocity = _agent.velocity;

        if (velocity.sqrMagnitude > 0.0001f)
        {
            _lookDirection = velocity.normalized;
        }
        else if(enemyState == EnemyState.Chasing || enemyState == EnemyState.Attacking)
        {
            Vector2 toPlayer = _player.position - transform.position;
            _lookDirection = toPlayer.normalized;
        }

        UpdateFacingDirection(_lookDirection);
        RotateObj(_lookDirection);
    }

    private void UpdateFacingDirection(Vector2 dir)
    {
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            enemyFacingDirection = dir.x > 0 ? EnemyFacingDirection.Right : EnemyFacingDirection.Left;
        }
        else
        {
            enemyFacingDirection = dir.y > 0 ? EnemyFacingDirection.Up : EnemyFacingDirection.Down;
        }
        SetAnimationDirection(new Vector2(dir.x, dir.y));
        /*
        switch (enemyFacingDirection)
        {
            case EnemyFacingDirection.Up:

                break;
            
            case EnemyFacingDirection.Down:
                SetAnimationDirection(new Vector2(0, -1));
                break;
            
            case EnemyFacingDirection.Left:
                SetAnimationDirection(new Vector2(-1, 0));
                break;
            
            case EnemyFacingDirection.Right:
                SetAnimationDirection(new Vector2(1, 0));
                break;
        }*/
    }
    
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

    public void StopPatrolForDialogue()
    {
        
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

    public void SetNewWaypoints(List<Transform> newWaypoints)
    {
        waypoints = newWaypoints;
        canPatrol = true;
        ResumePatrol();
    }
    
    private void SetNextWaypoint()
    {
        if (!canPatrol)
            return;

        if (waypoints == null || waypoints.Count == 0)
        {
            enemyState = EnemyState.Idle;
            return;
        }

        enemyState = EnemyState.Patrolling;
        _agent.isStopped = false;

        if (randomOrder)
        {
            if (waypoints.Count == 1)
            {
                _currentWaypointIndex = 0;
            }
            else
            {
                int newWaypointIndex;

                do
                {
                    newWaypointIndex = Random.Range(0, waypoints.Count);
                }
                while (newWaypointIndex == _currentWaypointIndex);

                _currentWaypointIndex = newWaypointIndex;
            }
        }
        else
        {
            _currentWaypointIndex =
                (_currentWaypointIndex + 1) % waypoints.Count;
        }

        _target = waypoints[_currentWaypointIndex];

        if (_target != null && _agent.isOnNavMesh)
        {
            _agent.SetDestination(_target.position);
        }
    }

    private void CheckIfWaypointIsReached()
    {
        if (!canPatrol)
            return;

        if (_isWaiting)
            return;

        if (_agent.pathPending)
            return;

        if (!_agent.hasPath)
            return;

        if (_agent.remainingDistance <= _agent.stoppingDistance + 0.05f)
        {
            if (waitAtWaypoint)
            {
                _newWaitpoint = StartCoroutine(
                    WaitBeforeNextWaypoint(
                        Random.Range(waitDuration.x, waitDuration.y)
                    )
                );
            }
            else
            {
                SetNextWaypoint();
            }
        }
    }

    private IEnumerator WaitBeforeNextWaypoint(float duration)
    {
        _isWaiting = true;
        enemyState = EnemyState.Idle;
        _agent.isStopped = true;

        yield return new WaitForSeconds(duration);

        _isWaiting = false;
        _newWaitpoint = null;

        if (!canPatrol || enemyState == EnemyState.Dead || _isAggroed)
            yield break;

        _agent.isStopped = false;
        SetNextWaypoint();
    }
    
    #endregion

    #region Animation

    private void UpdateAnimator()
    {
        Vector2 velocity = _agent.desiredVelocity;

        bool isMoving = velocity.sqrMagnitude > 0.01f;
        bool isAttacking = _canAttack && enemyState != EnemyState.Dead;

        animator.SetFloat(
            HashMovementValue,
            isMoving || isAttacking ? 1f : 0f
        );

        if (isMoving)
        {
            Vector2 direction = velocity.normalized;

            animator.SetFloat(HashDirX, direction.x);
            animator.SetFloat(HashDirY, direction.y);

            UpdateFacingDirection(direction);
            RotateObj(direction);
        }
    }

    private void SetAnimationDirection(Vector2 direction)
    {
        animator.SetFloat(HashDirX, direction.x);
        animator.SetFloat(HashDirY, direction.y);
    }

    private void SetAnimationAction(int actionId)
    {
        animator.SetTrigger(HashActionTrigger);
        animator.SetInteger(HashActionId, actionId);
    }

    #endregion

    #region Aggro

    public void EnterAggroDistance()
    {
        Debug.Log($"ENTER AGGRO: {gameObject.name}");
        _isAggroed = true;
        enemyState = EnemyState.Chasing;
        _target = _player;

        if (_aggroCoroutine != null)
        {
            StopCoroutine(_aggroCoroutine);
            _aggroCoroutine = null;
        }

        if (_newWaitpoint != null)
        {
            StopCoroutine(_newWaitpoint);
            _newWaitpoint = null;
            _isWaiting = false;
        }

        _agent.isStopped = false;
    }

    public void ExitAggroDistance()
    {
        Debug.Log($"EXIT AGGRO: {gameObject.name}");
        if (!isActiveAndEnabled || enemyState == EnemyState.Dead)
        {
            return;
        }

        _isAggroed = false;

        if (_aggroCoroutine != null)

        {
            StopCoroutine(_aggroCoroutine);
        }

        _aggroCoroutine = StartCoroutine(InitiateAggroTimer());
    }

    IEnumerator InitiateAggroTimer()
    {
        yield return new WaitForSeconds(stopChasingTimer);

        if (!_isAggroed && enemyState != EnemyState.Dead)
        {
            _target = null;
            _aggroCoroutine = null;
            if (canPatrol && waypoints != null && waypoints.Count > 0)
            {
                SetNextWaypoint();
            }
            else
            {
                enemyState = EnemyState.Idle;
                _agent.ResetPath();
            }
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
        if (!isActiveAndEnabled || enemyState == EnemyState.Dead)
            return;

        _canAttack = false;
        _attackCooldownTimer = 0f;

        if (_isAggroed)
        {
            enemyState = EnemyState.Chasing;
            _target = _player;
            _agent.isStopped = false;
        }
    }

    public void EndAttack()
    {
        if (enemyState == EnemyState.Dead)
            return;

        _attackCooldownTimer = 0f;

        if (_canAttack)
        {
            // Player досі поруч — слайм продовжує стрибати/атакувати.
            enemyState = EnemyState.Chasing;
            _agent.isStopped = true;
        }
        else if (_isAggroed)
        {
            enemyState = EnemyState.Chasing;
            _agent.isStopped = false;
        }
        else if (canPatrol && waypoints != null && waypoints.Count > 0)
        {
            SetNextWaypoint();
        }
        else
        {
            enemyState = EnemyState.Idle;
        }
    }
    
    #endregion
    
    public void Die()
    {
        if (enemyState == EnemyState.Dead)
            return;

        Debug.Log($"Death starts: {gameObject.name}");

        enemyState = EnemyState.Dead;

        _isAggroed = false;
        _canAttack = false;
        canPatrol = false;

        if (_aggroCoroutine != null)
        {
            StopCoroutine(_aggroCoroutine);
            _aggroCoroutine = null;
        }

        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        if (_newWaitpoint != null)
        {
            StopCoroutine(_newWaitpoint);
            _newWaitpoint = null;
        }

        StopAllCoroutines();

        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        Collider2D bodyCollider = GetComponent<Collider2D>();

        if (bodyCollider != null)
        {
            bodyCollider.enabled = false;
        }

        animator.ResetTrigger(HashActionTrigger);
        animator.SetFloat(HashMovementValue, 0f);
        animator.SetTrigger(HashDeathTrigger);

        Debug.Log($"Death trigger sent to {animator.gameObject.name}");
    }
    
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
