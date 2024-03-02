using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// The time it takes for the enemy to investigate the player & spot them 
    /// </summary>
    private const float InvestigationTime = 1.5f;

    #region Master Mode

    private const float MasterDetectionMultiplier = .75f;

    private const float MasterDetectionRangeMultiplier = 1.5f;

    private const float MasterFireRateMultiplier = 2f;

    #endregion

    #region Fields

    /// <summary>
    /// The target player 
    /// </summary>
    private GameObject _targetPlayer;

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

    /// <summary>
    /// Can the enemy shoot the player?
    /// </summary>
    private bool _canShoot = true;

    /// <summary>
    /// The projectile prefab 
    /// </summary>
    [Header("Shooting")] [SerializeField] private GameObject _projectilePrefab;

    /// <summary>
    /// // The rate at which the enemy can shoot (Bullets per minute)
    /// </summary>
    [SerializeField] private float _fireRate;

    [Header("Audio")] [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _fireAudioClip;

    [SerializeField] private AudioClip _frozenAudioClip;

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

    /// <summary>
    /// Is the enemy frozen
    /// </summary>
    public bool IsFrozen => _isFrozen;
    
    public float SpottingProgress => _timeInvestigating / InvestigationTime;

    #endregion

    // Update is called once per frame
    private void Update()
    {
        // TODO: Delete Later
        ChangeColorBasedOnState();

        // If the enemy is frozen, return
        if (_isFrozen)
            return;

        // Determine the target player
        DetermineTarget();

        // Determine the Patrol State
        DeterminePatrolState();

        // Shoot if the player is within range
        ShootIfWithinRange();
    }

    private void ChangeColorBasedOnState()
    {
        var material = GetComponent<Renderer>().material;

        switch (_patrolState)
        {
            case EnemyPatrolState.Patrol:
                material.color = Color.white;
                break;

            case EnemyPatrolState.Investigate:
                material.color = Color.yellow * (_timeInvestigating / InvestigationTime);
                break;

            case EnemyPatrolState.Chase:
                material.color = Color.red;
                break;

            case EnemyPatrolState.Lost:
                material.color = Color.red * (_timeInvestigating / InvestigationTime);
                break;

            case EnemyPatrolState.Idle:
                material.color = Color.white;
                break;

        }
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
        float difficultyMultiplier = ButtonStateManager.IsMasterButtonFilled
            ? MasterDetectionRangeMultiplier
            : 1;

        // Cast a ray from the enemy to the player
        var rayHit = Physics.Raycast(transform.position, player.transform.position - transform.position,
            out RaycastHit hit, visionRange * difficultyMultiplier);

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

        // If the target player null, return
        if (_targetPlayer == null) 
            return;
        
        // check if the player is still visible and save it to a variable
        bool isPlayerVisible = IsPlayerVisible(_targetPlayer);

        // Update the time since the player was last seen
        if (isPlayerVisible)
        {
            // Update the last known position of the player
            _lastKnownPlayerPosition = _targetPlayer.transform.position;

            // Create a multiplier for the difficulty
            float difficultyMultiplier = ButtonStateManager.IsMasterButtonFilled
                ? MasterDetectionMultiplier
                : 1;

            // If the player is visible, increment and clamp the time investigating
            _timeInvestigating = Mathf.Clamp(_timeInvestigating + (Time.deltaTime * difficultyMultiplier), 0,
                InvestigationTime);

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
            // Create a multiplier for the difficulty (divide this time)
            float difficultyMultiplier = ButtonStateManager.IsMasterButtonFilled
                ? MasterDetectionMultiplier
                : 1;

            // If the player is not visible, decrement the time investigating
            _timeInvestigating = Mathf.Clamp(_timeInvestigating - (Time.deltaTime / difficultyMultiplier), 0,
                InvestigationTime);

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

        // If the enemy has not spotted the player recently, return
        if (_timeInvestigating <= 0) 
            return;
        
        // If the enemy has been investigating for more than the investigation time,
        // set the patrol state to chase
        if (_timeInvestigating >= InvestigationTime)
            _patrolState = EnemyPatrolState.Chase;

        // The enemy has been investigating for less than the investigation time

        // If the enemy is currently losing the player, set the patrol state to lost
        else if (_losingTarget)
            _patrolState = EnemyPatrolState.Lost;

        // If the enemy is not losing the player, but their investigation time is less than the investigation time,
        // then they are investigating
        else
            _patrolState = EnemyPatrolState.Investigate;
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
        {
            StartCoroutine(FlashWhileFrozen());

            // Play the frozen audio
            _audioSource.PlayOneShot(_frozenAudioClip);
        }
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

    private void ShootIfWithinRange()
    {
        // If the enemy is not chasing the player, return
        if (_patrolState != EnemyPatrolState.Chase)
            return;

        // If the target player is null, return
        if (_targetPlayer == null)
            return;

        // If the target player is not visible, return
        if (!IsPlayerVisible(_targetPlayer))
            return;

        // Create a multiplier for the difficulty
        float difficultyMultiplier = ButtonStateManager.IsMasterButtonFilled
            ? MasterDetectionRangeMultiplier
            : 1;

        // If the player is not within range, return
        if (Vector3.Distance(transform.position, _targetPlayer.transform.position) > visionRange * difficultyMultiplier)
            return;

        // If the enemy cannot shoot, return
        if (!_canShoot)
            return;

        // Look at the player
        LookTowardTarget();

        // Instantiate the projectile prefab
        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);

        // Get the enemy projectile script
        EnemyProjectileScript enemyProjectileScript = projectile.GetComponent<EnemyProjectileScript>();

        // Fire the projectile        
        enemyProjectileScript.Fire(transform.forward);

        // Play the fire audio
        _audioSource.PlayOneShot(_fireAudioClip);

        // Start the reset can shoot coroutine
        StartCoroutine(ResetCanShoot());
    }

    // Coroutine to reset the can shoot variable
    private IEnumerator ResetCanShoot()
    {
        _canShoot = false;

        // 70 bullets / 1 minute * 1 minute / 60 seconds = 70 bullets / 60 seconds = 7 / 6 bullets / second

        float difficultyMultiplier = ButtonStateManager.IsMasterButtonFilled
            ? MasterFireRateMultiplier
            : 1;

        float bulletsPerSecond = (_fireRate * difficultyMultiplier) / 60f;

        // Wait for the cooldown time
        yield return new WaitForSeconds(1 / bulletsPerSecond);

        // Set can shoot to true
        _canShoot = true;
    }
}