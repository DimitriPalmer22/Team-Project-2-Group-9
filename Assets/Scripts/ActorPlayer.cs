using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorPlayer : Actor
{

    [Header("Hit Audio")]

    [SerializeField] private AudioSource _hitAudioSource;
    
    [SerializeField] private AudioClip _hitAudioClip;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ChangeHealth(int amount)
    {
        // If the novice button is filled, the player cannot lose health
        if (ButtonStateManager.IsNoviceButtonFilled)
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
