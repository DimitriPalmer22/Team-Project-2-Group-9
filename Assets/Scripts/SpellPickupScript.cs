using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpellPickupScript : MonoBehaviour
{

    #region fields

    /// <summary>
    /// The type of spell that the player will pick up
    /// </summary>
    [SerializeField] private SpellCastType spellType;

    #endregion fields
    
    // Property for the _spellType field
    public SpellCastType SpellType => spellType;

    #region Unity Methods

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // Pick up the spell
    //         PickUpSpell(_spellType);
    //         // Destroy the spell object
    //         Destroy(gameObject);
    //     }
    // }

    #endregion Unity Methods

    #region Methods

    /// <summary>
    /// Picks up the spell and sets the player's spell type
    /// </summary>
    /// <param name="spellCastScript">The script of the SpellCastScript calling this object</param>
    public void PickUpSpell(SpellCastScript spellCastScript)
    {
        // Set the player's spell type
        spellCastScript.PickUpSpell(spellType);
                
        // Debug Log that the player picked up this object's spell type
        Debug.Log($"Player picked up: {spellType}");
        
        // Destroy this game object
        Destroy(gameObject);
    }

    #endregion Methods
    
}
