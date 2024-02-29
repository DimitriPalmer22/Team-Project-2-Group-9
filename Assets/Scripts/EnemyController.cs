using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // The time it takes for the enemy to lose the player
    private const float LosePlayerTime = 3f;
    
    #region Fields
    
    /// <summary>
    /// The target player 
    /// </summary>
    private GameObject _targetPlayer;

    /// <summary>
    /// How long it has been since the player was last seen 
    /// </summary>
    private float _timeSincePlayerLastSeen;

    /// <summary>
    /// The range at which the enemy can see players
    /// </summary>
    [SerializeField] private float visionRange = 10f;

    /// <summary>
    /// Is the enemy frozen by the player's spell?
    /// </summary>
    private bool _isFrozen;
    
    #endregion Fields

    #region Properties

    /// <summary>
    /// Is the player spotted by the enemy?
    /// </summary>
    public bool IsPlayerSpotted => _targetPlayer != null;
    
    /// <summary>
    /// The target player
    /// </summary>
    public GameObject TargetPlayer => _targetPlayer;

    #endregion
    
    // Update is called once per frame
    void Update()
    {
        // If the enemy is frozen, return
        if (_isFrozen)
            return;
        
        // Determine the target player
        DetermineTarget();
        
        // // Look toward the target player
        // LookTowardTarget();
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
                _timeSincePlayerLastSeen = 0;
            else
            {
                _timeSincePlayerLastSeen += Time.deltaTime;
                
                // If the player has been lost for too long, set the target player to null
                if (_timeSincePlayerLastSeen > LosePlayerTime)
                {
                    _targetPlayer = null;
                    _timeSincePlayerLastSeen = 0;
                }
            }
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
        
        // if the player was last seen more than 0 seconds ago, return
        if (_timeSincePlayerLastSeen > 0)
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
