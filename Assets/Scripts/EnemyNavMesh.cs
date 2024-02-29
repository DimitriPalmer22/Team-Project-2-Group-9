using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{

    private const float PatrolCheckpointDistance = 3f;
    
    #region Fields

    /// <summary>
    /// The target player / whatever the enemy is navigating to
    /// </summary>
    private Transform _navigationTarget;

    /// <summary>
    /// The enemy controller component
    /// </summary>
    private EnemyController _enemyController;
    
    /// <summary>
    /// The nav mesh agent component
    /// </summary>
    private NavMeshAgent _navMeshAgent;

    /// <summary>
    /// The checkpoints the enemy will walk to during their patrol
    /// </summary>
    [SerializeField] private Transform[] _patrolCheckpoints;

    /// <summary>
    /// The index of the current patrol checkpoint the enemy is moving to.
    /// DO NOT CHANGE THIS IN THE INSPECTOR
    /// TODO: Make this private
    /// </summary>
    [SerializeField] private int _currentPatrolIndex;
    
    /// <summary>
    /// The state of the enemy's patrol
    /// </summary>
    [SerializeField] private EnemyPatrolState _patrolState;
    
    #endregion

    #region Unity Methods

    
    private void Awake()
    {
        // get the nav mesh agent component
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // get the enemy controller component
        _enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Make a transform object that follows the player's position while they are spotted

        // Determine the patrol state
        DeterminePatrolState();

        switch (_patrolState)
        {
            // if the enemy is patrolling, navigate to the current checkpoint
            case EnemyPatrolState.Patrol:
                NavigateIfPatrolling();
                break;
            
            // if the enemy is investigating, move slowly, but don't shoot
            case EnemyPatrolState.Investigate:
                break;

            // if the enemy is chasing the player, navigate to the player
            case EnemyPatrolState.Chase:
                NavigateIfPlayerSpotted();
                break;
            
            // if the enemy has lost the player, navigate to the last known location
            case EnemyPatrolState.Lost:
                break;
            
            case EnemyPatrolState.Idle:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        // set the destination of the nav mesh agent to the navigation target
        if (_navigationTarget != null && _navMeshAgent != null && _navMeshAgent.enabled)
            _navMeshAgent.destination = _navigationTarget.position;
    }

    #endregion

    #region Methods

    private void NavigateIfPlayerSpotted()
    {
        // Set the nav mesh agent's destination to the player's position
        _navMeshAgent.destination = _enemyController.TargetPlayer.transform.position;
        
        _navMeshAgent.enabled = _enemyController.IsPlayerSpotted;
    }

    private void NavigateIfPatrolling()
    {
        // return if the patrol checkpoints array is empty
        if (_patrolCheckpoints.Length == 0)
            return;
        
        // modulus the current patrol index by the length of the patrol checkpoints array (Avoid IndexOutOfRangeException)
        _currentPatrolIndex %= _patrolCheckpoints.Length;
        
        // if the enemy is close to the current patrol checkpoint, increment the current patrol index
        if (Vector3.Distance(transform.position, _patrolCheckpoints[_currentPatrolIndex].position) < PatrolCheckpointDistance)
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolCheckpoints.Length;
        
        // set the nav mesh agent's destination to the current patrol checkpoint
        _navMeshAgent.destination = _patrolCheckpoints[_currentPatrolIndex].position;
        
        // if the enemy is patrolling, enable the nav mesh agent
        if (_patrolState == EnemyPatrolState.Patrol)
            _navMeshAgent.enabled = true;
    }

    private void DeterminePatrolState()
    {
        // if (_enemyController.IsPlayerSpotted)
        //     _patrolState = EnemyPatrolState.Chase;
        //
        // else
        // {
        //     _patrolState = EnemyPatrolState.Patrol;
        // }
    }

    #endregion
    
}

/// <summary>
/// The state of the enemy's patrol
/// </summary>
public enum EnemyPatrolState
{
    Patrol,
    Investigate,
    Chase,
    Lost,
    Idle,
}