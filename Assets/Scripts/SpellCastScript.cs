using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for casting spells.
/// Should only be applied to the player
/// </summary>
public class SpellCastScript : MonoBehaviour
{
    #region fields
    
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
    
    // A SpellPickupScript object that the player is looking at 
    private SpellPickupScript _spellPickupScript;
    
    // The camera
    private Camera _camera;
    
    #endregion fields
    
    #region Keys
    
    // The key to cast the spell
    private const KeyCode CastSpellKey = KeyCode.F;

    // The key to pick up a spell
    private const KeyCode PickupSpellKey = KeyCode.E;
    
    #endregion Keys
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the camera to the main camera
        _camera = Camera.main;
        
        // Pick up a freeze spell by default
        // ! TODO: Delete later
        PickUpSpell(SpellCastType.Freeze);
    }

    // Update is called once per frame
    void Update()
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
            
            // Debug Log the remaining duration of the spell
            Debug.Log($"Spell effect remaining: {_spellEffectRemaining}");
        }
    }

    /// <summary>
    /// Read the player's input
    /// </summary>
    void UpdateInput()
    {
        // If the player presses the spell cast button, cast the spell
        if (Input.GetKeyDown(CastSpellKey))
            CastSpell();
        
        // Determine which spell the player is looking at
        DetermineLookedAtSpell();
        
        // If the player presses the spell pickup button, pick up the spell
        if (Input.GetKeyDown(PickupSpellKey) && _spellPickupScript != null)
            _spellPickupScript.PickUpSpell(this);
        
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
                break;
            
            // Invisibility spell
            case SpellCastType.Invisibility:
                // Apply the invisibility effect
                break;
            
            // Teleport spell
            case SpellCastType.Teleport:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        // Decrement the remaining uses of this spell
        _remainingUses -= 1;
        
        // Set the spell to active
        _isSpellActive = true;
        
        // Debug Log the current spell type and the remaining number of uses
        Debug.Log($"Casting spell: {_spellType}. Remaining uses: {_remainingUses}");
    }

    /// <summary>
    /// Deactivate the current spell if it has a duration
    /// </summary>
    private void DeactivateSpell()
    {
        // Set the remaining spell duration to 0
        _spellEffectRemaining = 0;
        
        // Different effects depending on the current spell type
        switch (_spellType)
        {
            // Invisibility spell
            case SpellCastType.Invisibility:
                // Remove the invisibility effect
                break;
            
            // Teleport spell
            case SpellCastType.Teleport:
                break;
            
            default:
                break;
        }
        
        // set the spell to inactive
        _isSpellActive = false;
    }


    /// <summary>
    /// Determine which spell the player is looking at
    /// </summary>
    private void DetermineLookedAtSpell()
    {
        // Get the camera's transform
        var cameraTransform = _camera.transform;
        
        // RayCast to determine if the player is looking at a GameObject tagged "Spell Pickup"
        var hitAGameObject = Physics.Raycast(
            cameraTransform.position, 
            cameraTransform.forward, out var hit, 10);

        // If the player is not looking at a GameObject, return
        if (!hitAGameObject) 
            return;

        // If the player is not looking at a spell, return
        if (!hit.collider.CompareTag("Spell Pickup")) 
            return;
        
        // Set the _spellPickupScript to the SpellPickupScript of the object the player is looking at
        _spellPickupScript = hit.collider.GetComponent<SpellPickupScript>();
    }
    
    public void PickUpSpell(SpellCastType spellType)
    {
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
                _remainingUses = 2;
                break;
            
            // Picked up an invisibility spell
            case SpellCastType.Invisibility:
                _remainingUses = 1;
                _spellEffectRemaining = 10;
                break;
            
            // Picked up a teleport spell
            case SpellCastType.Teleport:
                _remainingUses = 1;
                break;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(spellType), spellType, null);
        }
        
        // Debug Log the current spell type and the remaining number of uses
        Debug.Log($"Picked up spell: {spellType}. Remaining uses: {_remainingUses}");
    }
    
}