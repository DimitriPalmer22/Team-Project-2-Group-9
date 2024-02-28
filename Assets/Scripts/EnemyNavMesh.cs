using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private Transform _navigationTarget;

    private EnemyController _enemyController;
    
    private NavMeshAgent _navMeshAgent;

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
        // navigate if the player is spotted
        NavigateIfPlayerSpotted();
        
        // set the destination of the nav mesh agent to the navigation target
        if (_navigationTarget != null && _navMeshAgent != null && _navMeshAgent.enabled)
            _navMeshAgent.destination = _navigationTarget.position;
    }

    private void NavigateIfPlayerSpotted()
    {
        _navMeshAgent.enabled = _enemyController.IsPlayerSpotted;
    }
    
}