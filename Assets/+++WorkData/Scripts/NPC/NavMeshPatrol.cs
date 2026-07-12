//Volodymyr Pavlik
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPatrol : MonoBehaviour
{
    #region Inspector

    [Header("NPC Reference")] 
    [SerializeField] private Animator animator;
    private int HashMovementValue = Animator.StringToHash("MovementValue");
    private int HashDirX = Animator.StringToHash("dirX");
    private int HashDirY = Animator.StringToHash("dirY");
    private static readonly int HashStopFishing = Animator.StringToHash("StopFishing");
    [SerializeField] private bool useStartAnimationTrigger;
    [SerializeField] private string startAnimationTrigger;
    [SerializeField] private bool startDirectionIsRight = false;

    [Header("Waypoints")] 
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private bool waitAtWaypoint = true;
    [SerializeField] private bool randomOrder;
    [SerializeField] private bool canPatrol = true;
    [SerializeField] private Vector2 waitDuration = new Vector2(1, 5);

    #endregion
    
    #region Private Variables

    private NavMeshAgent _agent;
    private int _currentWaypointIndex = -1;
    private bool _isWaiting;
    private Transform _target;

    #endregion
    
    #region Unity Event Functions

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = waitAtWaypoint;
    }

    private void Start()
    {
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        
        if (canPatrol)
        {
            SetNextWaypoint();
        }
        else
        {
            _agent.isStopped = true;
        }
    }

    private void Update()
    {
        if (canPatrol)
        {
            if (_agent.isStopped) return;
            
            CheckIfWaypointIsReached();
        }
        
        if (!_agent.isStopped && !_agent.pathPending && _target)
        {
            _agent.SetDestination(_target.position);
        }

        Vector2 direction = _agent.velocity;

        UpdateAnimator(direction);
        RotateObj(direction);
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
        _isWaiting = true;
        yield return new WaitForSeconds(duration);
        _isWaiting = false;
        SetNextWaypoint();
    }
    
    #endregion

    private void UpdateAnimator(Vector2 direction)
    {
        float movementValue = direction.sqrMagnitude > 0.01f ? 1f : 0f;

        animator.SetFloat(HashMovementValue, movementValue);

        if (movementValue > 0)
        {
            animator.SetFloat(HashDirX, direction.normalized.x);
            animator.SetFloat(HashDirY, direction.normalized.y);
        }
    }
    
    public void StartPatrol()
    {
        if (!_agent.enabled)
        {
            _agent.enabled = true;
        }

        enabled = true;
        canPatrol = true;

        if (_agent.isOnNavMesh)
        {
            _agent.isStopped = false;
            SetNextWaypoint();
        }

        if (useStartAnimationTrigger &&
            !string.IsNullOrWhiteSpace(startAnimationTrigger))
        {
            animator.SetTrigger(startAnimationTrigger);
        }
    }
}
