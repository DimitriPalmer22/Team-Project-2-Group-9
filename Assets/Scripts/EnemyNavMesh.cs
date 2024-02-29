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
    
    #endregion

    #region Properties

    /// <summary>
    /// The patrol state from the enemy controller
    /// </summary>
    private EnemyPatrolState PatrolState => _enemyController.PatrolState;

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

        switch (PatrolState)
        {
            // if the enemy is patrolling, navigate to the current checkpoint
            case EnemyPatrolState.Patrol:
                NavigateIfPatrolling();
                break;
            
            // if the enemy is investigating, move slowly, but don't shoot
            case EnemyPatrolState.Investigate:
                NavigateIfInvestigating();
                break;

            // if the enemy is chasing the player, navigate to the player
            case EnemyPatrolState.Chase:
                NavigateIfChasing();
                break;
            
            // if the enemy has lost the player, navigate to the last known location
            case EnemyPatrolState.Lost:
                NavigateIfLost();
                break;
            
            case EnemyPatrolState.Idle:
                _navMeshAgent.enabled = false;
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

    private void NavigateIfChasing()
    {
        // return if the patrol state is not chase
        if (PatrolState != EnemyPatrolState.Chase)
            return;

        // set the navigation target to the player's transform
        _navigationTarget = _enemyController.TargetPlayer.transform;
        
        // Set the nav mesh agent's destination to the player's position
        _navMeshAgent.destination = _enemyController.TargetPlayer.transform.position;
        
        // enable the nav mesh agent
        _navMeshAgent.enabled = _enemyController.IsPlayerSpotted;
        
    }

    private void NavigateIfPatrolling()
    {
        // return if the patrol state is not patrol
        if (PatrolState != EnemyPatrolState.Patrol)
            return;
        
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
        
        // enable the nav mesh agent
        _navMeshAgent.enabled = true;
    }

    private void NavigateIfLost()
    {
        // return if the patrol state is not lost
        if (PatrolState != EnemyPatrolState.Lost)
            return;
        
        // if the enemy is lost, navigate to the last known player position
        _navMeshAgent.destination = _enemyController.LastKnownPlayerPosition;
        
        // enable the nav mesh agent
        _navMeshAgent.enabled = true;
    }
    
    private void NavigateIfInvestigating()
    {
        // return if the patrol state is not investigate
        if (PatrolState != EnemyPatrolState.Investigate)
            return;
        
        // set the navigation target to the player's transform
        _navigationTarget = _enemyController.TargetPlayer.transform;
        
        // if the enemy is investigating, navigate to the last known player position
        _navMeshAgent.destination = _enemyController.LastKnownPlayerPosition;
        
        // enable the nav mesh agent
        _navMeshAgent.enabled = true;
    }
    
    public void SetNearestPatrolTarget()
    {
        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;
        
        for (int i = 0; i < _patrolCheckpoints.Length; i++)
        {
            // get the distance between the enemy and the current patrol checkpoint
            float distance = Vector3.Distance(transform.position, _patrolCheckpoints[i].position);
            
            // if the distance is less than the nearest distance, set the nearest index to the current index
            if (distance < nearestDistance)
            {
                nearestIndex = i;
                nearestDistance = distance;
            }
        }
        
        // set the current patrol index to the nearest index
        _currentPatrolIndex = nearestIndex;
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