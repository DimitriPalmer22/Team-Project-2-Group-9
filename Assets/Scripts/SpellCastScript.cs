using System.Collections;
using UnityEngine;

/// <summary>
/// Script used for casting spells.
/// Should only be applied to the player
/// </summary>
public class SpellCastScript : MonoBehaviour
{
    /// <summary>
    /// How long the freeze spell will last
    /// </summary>
    private const float FreezeDuration = 3;
    
    // The distance from which the player can pick up a spell
    public const float PickupDistance = 3;

    // The number of particles to emit
    private const int ParticlesAmount = 200;
    
    // The duration of the invisibility spell
    private const float InvisibilityDuration = 8;
    
    // The number of uses given when the player picks up an invisibility spell
    private const int InvisibilityUses = 1;
    
    // The number of uses given when the player picks up a freeze spell
    private const int FreezeUses = 2;
    
    #region Fields
    
    /// <summary>
    /// which spell the player is currently using
    /// </summary>
    private SpellCastType _spellType;

    /// <summary>
    /// The number of times the user can cast the spell
    /// </summary>
    private int _remainingUses;

    /// <summary>
    /// How much longer the spell's effect will last
    /// </summary>
    private float _spellEffectRemaining;

    /// <summary>
    /// Boolean to determine if the spell is currently active
    /// </summary>
    private bool _isSpellActive;

    [Header("Freeze Spell")]
    [SerializeField] private GameObject _freezePrefab;
    [SerializeField] private AudioSource _freezeSource;
    [SerializeField] private AudioClip _freezeClip;
    [SerializeField] private ParticleSystem _freezeParticles;
    
    [Header("Invisibility Spell")]
    [SerializeField] private AudioSource _invisibilitySource;
    [SerializeField] private AudioClip _invisibilityClip;
    [SerializeField] private ParticleSystem _invisibilityParticles;
    
    // The camera
    private Camera _camera;
    
    #endregion Fields
    
    #region Properties
    
    public SpellCastType SpellType => _spellType;
    
    public int RemainingUses => _remainingUses;
    
    public bool IsInvisible => _spellType == SpellCastType.Invisibility && _isSpellActive;
    
    public float RemainingDuration => _spellEffectRemaining;
    
    public bool IsSpellActive => _isSpellActive;
    
    #endregion Properties
    
    #region Keys
    
    // The key to cast the spell
    private const KeyCode CastSpellKey = KeyCode.F;

    // The key to pick up a spell
    public const KeyCode PickupSpellKey = KeyCode.E;
    
    #endregion Keys
    
    // Start is called before the first frame update
    private void Start()
    {
        // Set the camera to the main camera
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        // Read the player's input
        UpdateInput();
        
        // Tick down the spell duration if the spell is active
        if (_isSpellActive)
        {
            _spellEffectRemaining -= Time.deltaTime;
            
            // if the spell's effect has expired, deactivate the spell
            if (_spellEffectRemaining <= 0)
                DeactivateSpell();
        }

        // If the player has no uses left, set the spell type to none
        if (_spellType != SpellCastType.None && _remainingUses <= 0)
        {
            // if the current spell is not a duration spell
            if (!_spellType.IsDurationSpellType())
                _spellType = SpellCastType.None;
            
            else if (_spellEffectRemaining <= 0)
                _spellType = SpellCastType.None;
        }
    }

    #region Methods
    
    /// <summary>
    /// Read the player's input
    /// </summary>
    private void UpdateInput()
    {
        // Disable input if the game is over
        if (GlobalScript.Instance.IsGameOver)
            return;
        
        // If the player presses the spell cast button, cast the spell
        if (Input.GetKeyDown(CastSpellKey))
            CastSpell();
        
        // If the player presses the spell pickup button, pick up the spell
        if (Input.GetKeyDown(PickupSpellKey))
        {
            // Determine which spell the player is looking at
            var spellPickupScript = DetermineLookedAtSpell();

            // If the player is looking at a spell, pick it up
            if (spellPickupScript != null)
                spellPickupScript.PickUpSpell(this);
        }
        
    }
    
    /// <summary>
    /// Activate the current spell
    /// </summary>
    private void CastSpell()
    {
        // Don't allow the user to use a spell if they have no uses left
        if (_remainingUses <= 0)
            return;

        // Based on the spell type, use different casting logic
        switch (_spellType)
        {
            // No active spell
            case SpellCastType.None:
                break;
            
            // Freeze spell
            case SpellCastType.Freeze:
                // Apply the freeze effect
                
                // Instantiate the freeze projectile
                var freezeProjectile = Instantiate(_freezePrefab, _camera.transform.position, Quaternion.identity);

                // Get the freeze projectile script
                var freezeProjectileScript = freezeProjectile.GetComponent<FreezeProjectileScript>();
                
                // Fire the freeze projectile
                freezeProjectileScript.Fire(this, _camera.transform.forward);
                
                // Play the freeze sound
                _freezeSource.PlayOneShot(_freezeClip);
                
                // Play the freeze particles
                _freezeParticles.Emit(ParticlesAmount);
                
                break;
            
            // Invisibility spell
            case SpellCastType.Invisibility:
                // Apply the invisibility effect
                
                // Play the invisibility sound
                _invisibilitySource.PlayOneShot(_invisibilityClip);
                
                // Play the invisibility particles
                _invisibilityParticles.Emit(ParticlesAmount);
                
                break;
            
        }
        
        // Decrement the remaining uses of this spell
        _remainingUses -= 1;
        
        // Set the spell to active
        _isSpellActive = true;
        
        // Debug Log the current spell type and the remaining number of uses
        Debug.Log($"Casting spell: {_spellType}. Remaining uses: {_remainingUses}");
    }

    /// <summary>
    /// Deactivate the current spell
    /// </summary>
    private void DeactivateSpell()
    {
        // Set the remaining spell duration to 0
        _spellEffectRemaining = 0;
        
        // Don't deactivate the spell if it's not active
        if (!_isSpellActive)
            return;
        
        // set the spell to inactive
        _isSpellActive = false;
        
    }


    /// <summary>
    /// Determine which spell the player is looking at
    /// </summary>
    private SpellPickupScript DetermineLookedAtSpell()
    {
        // Get the camera's transform
        var cameraTransform = _camera.transform;
        
        // RayCast to determine if the player is looking at a GameObject tagged "Spell Pickup"
        var hitAGameObject = Physics.Raycast(
            cameraTransform.position, 
            cameraTransform.forward, out var hit, PickupDistance);

        // If the player is not looking at a GameObject, return
        if (!hitAGameObject) 
            return null;

        // If the player is not looking at a spell, return
        if (!hit.collider.CompareTag("Spell Pickup")) 
            return null;
        
        // return the SpellPickupScript of the object the player is looking at
        return hit.collider.GetComponent<SpellPickupScript>();
    }
    
    /// <summary>
    /// Set the player's spell type & set their spell-related fields
    /// </summary>
    /// <param name="spellType"></param>
    public void PickUpSpell(SpellCastType spellType)
    {
        // Deactivate the player's current spell
        DeactivateSpell();
        
        // Set the current spell type to the one the player picked up
        _spellType = spellType;
        
        // Reset the number of spells remaining and the effect duration.
        // Then, update those values, if necessary, within the switch statement.
        // ! NOTE: This WILL DISABLE any active spells
        _spellEffectRemaining = 0;
        _remainingUses = 0;

        // Based on which spell the player picks up, apply a different effect
        switch (spellType)
        {
            // No active spell
            case SpellCastType.None:
                _remainingUses = 0;
                break;
            
            // Picked up a freeze spell
            case SpellCastType.Freeze:
                _remainingUses = FreezeUses;
                break;
            
            // Picked up an invisibility spell
            case SpellCastType.Invisibility:
                _remainingUses = InvisibilityUses;
                _spellEffectRemaining = InvisibilityDuration;
                break;
            
            // Picked up a teleport spell
            case SpellCastType.Teleport:
                _remainingUses = 1;
                break;
        }
        
        // Debug Log the current spell type and the remaining number of uses
        Debug.Log($"Picked up spell: {spellType}. Remaining uses: {_remainingUses}");
    }
    
    /// <summary>
    /// Freeze the enemy for several seconds
    /// </summary>
    /// <param name="enemyController"></param>
    /// <returns></returns>
    public IEnumerator FreezeEnemy(EnemyController enemyController)
    {
        // if enemyController is null, return
        if (enemyController == null)
            yield break;
        
        // Debug Log that the enemy is frozen
        Debug.Log("Freezing the enemy");
        
        // Freeze the enemy
        enemyController.SetFrozen(true);
        
        // Wait 3 seconds
        yield return new WaitForSeconds(FreezeDuration);
        
        // Unfreeze the enemy
        enemyController.SetFrozen(false);
        
        // Debug Log that the enemy is unfrozen
        Debug.Log("Unfreezing the enemy");
    }
    
    #endregion
    
}