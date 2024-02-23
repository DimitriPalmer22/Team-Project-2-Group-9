using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // The range at which the enemy can see players
    private const float VisionRange = 10f;
    
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
    
    #endregion Fields
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        // Debug log the current target player
        Debug.Log($"Target player: ({_targetPlayer}) ({_timeSincePlayerLastSeen} seconds since last seen).");
    }

    /// <summary>
    /// Look for the nearest player to the enemy
    /// </summary>
    void LookForNearestPlayer()
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
        var rayHit = Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit, VisionRange);
        
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
}
