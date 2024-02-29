using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    /// <summary>
    /// The time it takes for the enemy to lose the player 
    /// </summary>
    private const float LosePlayerTime = 3f;

    /// <summary>
    /// The time it takes for the enemy to investigate the player & spot them 
    /// </summary>
    private const float InvestigationTime = 1.5f;
    
    #region Fields
    
    /// <summary>
    /// The target player 
    /// </summary>
    private GameObject _targetPlayer;

    // /// <summary>
    // /// How long it has been since the player was last seen 
    // /// </summary>
    // private float _timeSincePlayerLastSeen;

    /// <summary>
    /// The time the enemy has been investigating the player
    /// </summary>
    private float _timeInvestigating;

    /// <summary>
    /// The range at which the enemy can see players
    /// </summary>
    [SerializeField] private float visionRange = 10f;

    /// <summary>
    /// Is the enemy frozen by the player's spell?
    /// </summary>
    private bool _isFrozen;
    
    /// <summary>
    /// The state of the enemy's patrol
    /// </summary>
    private EnemyPatrolState _patrolState;

    /// <summary>
    /// Is the enemy currently losing the target player?
    /// </summary>
    private bool _losingTarget;

    /// <summary>
    /// Was the enemy chasing the player before losing them? 
    /// </summary>
    private bool _wasChasingBeforeLosing;
    
    /// <summary>
    /// The last known position of the player
    /// </summary>
    private Vector3 _lastKnownPlayerPosition;
    
    #endregion Fields

    #region Properties

    /// <summary>
    /// Is the player spotted by the enemy?
    /// </summary>
    public bool IsPlayerSpotted => _targetPlayer != null && _timeInvestigating >= InvestigationTime;
    
    /// <summary>
    /// The target player
    /// </summary>
    public GameObject TargetPlayer => _targetPlayer;
    
    public EnemyPatrolState PatrolState => _patrolState;
    
    
    /// <summary>
    /// The last known position of the player
    /// </summary>
    public Vector3 LastKnownPlayerPosition => _lastKnownPlayerPosition;
    

    #endregion
    
    // Update is called once per frame
    void Update()
    {
        // TODO: Delete this later
        var material = GetComponent<Renderer>().material;

        switch (_patrolState)
        {
            case EnemyPatrolState.Patrol:
                material.color = Color.white;
                break;
            
            case EnemyPatrolState.Investigate:
                material.color = Color.yellow* (_timeInvestigating / InvestigationTime);
                break;
            
            case EnemyPatrolState.Chase:
                material.color = Color.red;
                break;
            
            case EnemyPatrolState.Lost:
                material.color = Color.red* (_timeInvestigating / InvestigationTime);
                break;
            
            case EnemyPatrolState.Idle:
                material.color = Color.white;
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
            
        
        // If the enemy is frozen, return
        if (_isFrozen)
            return;
        
        // Determine the target player
        DetermineTarget();
        
        // // Look toward the target player
        // LookTowardTarget();
        
        // Determine the Patrol State
        DeterminePatrolState();
        
    }

    /// <summary>
    /// Look for the nearest player to the enemy
    /// </summary>
    private void LookForNearestPlayer()
    {
        // Get all the visible players in the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player").Where(IsPlayerVisible).ToArray();
        
        // Use linq to order the players by distance to the enemy
        var sortedPlayers = players.OrderBy(player => Vector3.Distance(player.transform.position, transform.position));
        
        // If the sorted players list is empty, return
        if (!sortedPlayers.Any())
            return;
        
        // Use linq to find the closest player to this enemy
        GameObject closestPlayer = sortedPlayers.First();
        
        // Set the closest player as the target player
        _targetPlayer = closestPlayer;
    }

    /// <summary>
    /// Check if the player is visible to the enemy
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private bool IsPlayerVisible(GameObject player)
    {
        // Cast a ray from the enemy to the player
        var rayHit = Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit, visionRange);
        
        // If the ray hits something, check if it's the player
        if (!rayHit) 
            return false;
        
        // Test if the player is invisible using the player's SpellCastScript script
        var spellCastScript = player.GetComponent<SpellCastScript>();
        if (spellCastScript == null || spellCastScript.IsInvisible)
            return false;

        // If the ray hits the player, return true
        return hit.collider.gameObject == player;
    }

    /// <summary>
    /// Determine the target player for the enemy
    /// </summary>
    private void DetermineTarget()
    {
        // If the target player is null, look for the nearest player
        if (_targetPlayer == null)
            LookForNearestPlayer();
        
        // If the target player is not null
        if (_targetPlayer != null)
        {
            // check if the player is still visible and save it to a variable
            bool isPlayerVisible = IsPlayerVisible(_targetPlayer);

            // Update the time since the player was last seen
            if (isPlayerVisible)
            {
                // Update the last known position of the player
                _lastKnownPlayerPosition = _targetPlayer.transform.position;
                
                // // If the player is visible, reset the time since the player was last seen
                // _timeSincePlayerLastSeen = 0;
                
                // If the player is visible, increment and clamp the time investigating
                _timeInvestigating = Mathf.Clamp(_timeInvestigating + Time.deltaTime, 0, InvestigationTime);

                // If the enemy is currently losing the target, immediately finish investigating
                if (_losingTarget && _wasChasingBeforeLosing)
                    _timeInvestigating = InvestigationTime;
                
                // If the enemy has been investigating for more than the investigation time, 
                // set was chasing before losing to true
                if (_timeInvestigating >= InvestigationTime)
                    _wasChasingBeforeLosing = true;
                
                // If the player is visible, set the losing target player to false
                _losingTarget = false;
            }
            else
            {
                // If the player is not visible, decrement the time investigating
                _timeInvestigating = Mathf.Clamp(_timeInvestigating - Time.deltaTime, 0, InvestigationTime);

                _losingTarget = true;
                
                // If the player has been lost for too long, set the target player to null
                if (_timeInvestigating <= 0)
                {
                    // If the enemy has fully lost the target, reset losing target to false
                    _losingTarget = false;

                    // reset the target player
                    _targetPlayer = null;
                    
                    // reset was chasing before losing
                    _wasChasingBeforeLosing = false;
                    
                    // _timeSincePlayerLastSeen = 0;
                }
            }
        }

    }
    
    private void DeterminePatrolState()
    {
        // If the enemy has no target player, set the patrol state to patrol
        if (_targetPlayer == null && _patrolState != EnemyPatrolState.Patrol)
        {
            _patrolState = EnemyPatrolState.Patrol;
            GetComponent<EnemyNavMesh>().SetNearestPatrolTarget();
            return;
        }
        
        // If the enemy has spotted the player recently
        if (_timeInvestigating > 0)
        {
            // If the enemy has been investigating for more than the investigation time,
            // set the patrol state to chase
            if (_timeInvestigating >= InvestigationTime)
                _patrolState = EnemyPatrolState.Chase;
            
            // If the enemy has been investigating for less than the investigation time
            
            // If the enemy is currently losing the player, set the patrol state to lost
            else if (_losingTarget)
                _patrolState = EnemyPatrolState.Lost;
            
            // If the enemy is not losing the player, but their investigation time is less than the investigation time,
            // then they are investigating
            else
                _patrolState = EnemyPatrolState.Investigate;
            
            return;
        }
        
    }

    /// <summary>
    /// Look at the target player
    /// </summary>
    private void LookTowardTarget()
    {
        // If the target player is null, return
        if (_targetPlayer == null)
            return;
        
        // Look at the target player
        transform.LookAt(_targetPlayer.transform);
    }
    
    /// <summary>
    /// Set the enemy to frozen or unfrozen
    /// </summary>
    /// <param name="isFrozen"></param>
    public void SetFrozen(bool isFrozen)
    {
        _isFrozen = isFrozen;

        // If the enemy is now frozen, start the flash while frozen coroutine
        if (_isFrozen)
            StartCoroutine(FlashWhileFrozen());

    }

    /// <summary>
    /// Flash the enemy cyan while it is frozen
    /// </summary>
    private IEnumerator FlashWhileFrozen()
    {
        // The duration of each blink
        const float blinkDuration = .1f;
        
        // Get the enemy's material
        Material rendererMaterial = GetComponent<Renderer>().material;
        
        // While the enemy is frozen
        while (_isFrozen)
        {
            // Set the enemy's material to be cyan
            rendererMaterial.color = Color.cyan;
            
            // Wait for the blink duration
            yield return new WaitForSeconds(blinkDuration);

            // Set the enemy's material to be white
            rendererMaterial.color = Color.white;
            
            // Wait for the blink duration
            yield return new WaitForSeconds(blinkDuration);
        }
    }
    
}
