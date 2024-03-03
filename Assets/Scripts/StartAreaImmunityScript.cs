using UnityEngine;

public class StartAreaImmunityScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is the player
        if (!other.gameObject.CompareTag("Player"))
            return;

        // Get the spell cast script
        SpellCastScript spellCastScript = other.GetComponent<SpellCastScript>();
        
        // Set the player's immunity to true
        spellCastScript.IsInStartArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the other object is the player
        if (!other.gameObject.CompareTag("Player"))
            return;

        // Get the spell cast script
        SpellCastScript spellCastScript = other.GetComponent<SpellCastScript>();
        
        // Set the player's immunity to true
        spellCastScript.IsInStartArea = false;
        
    }
}
