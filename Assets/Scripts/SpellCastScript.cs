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
    
    #endregion fields
    
    #region Keys
    
    private const KeyCode CastSpellKey = KeyCode.E;
    
    #endregion Keys
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Read the player's input
        UpdateInput();
        
        // Tick the spell duration if the current spell type requires it
        if (_spellType.IsDurationSpellType())
            _spellEffectRemaining -= Time.deltaTime;
    }

    /// <summary>
    /// Read the player's input
    /// </summary>
    void UpdateInput()
    {
        // If the player presses the spell cast button, cast the spell
        if (Input.GetKeyDown(CastSpellKey))
            CastSpell();
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
        
        // Debug Log to show the player which spell they cast
        Debug.Log($"Casting spell: {_spellType}");
    }

    public void PickUpSpell(SpellCastType spellType)
    {
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
    }
    
}
