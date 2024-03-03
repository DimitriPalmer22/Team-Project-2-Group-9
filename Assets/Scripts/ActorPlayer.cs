using UnityEngine;

public class ActorPlayer : Actor
{

    [Header("Hit Audio")]

    [SerializeField] private AudioSource _hitAudioSource;
    
    [SerializeField] private AudioClip _hitAudioClip;
    
    public override void ChangeHealth(int amount)
    {
        // If the novice button is filled, the player cannot lose health
        if (ButtonStateManager.IsNoviceButtonFilled)
            return;
     
        // get the spell cast script
        SpellCastScript spellCastScript = GetComponent<SpellCastScript>();
        
        // If the player is immune, do nothing
        if (spellCastScript.IsInStartArea)
            return;
        
        base.ChangeHealth(amount);

        if (amount < 0 && _hitAudioSource != null)
        {
            // Play the hit audio
            _hitAudioSource.PlayOneShot(_hitAudioClip);
        }
    }

    protected override void Die()
    {
        GlobalScript.Instance.LoseGame();
    }
}
