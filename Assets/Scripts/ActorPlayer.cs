using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorPlayer : Actor
{
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
    }

    protected override void Die()
    {
        GlobalScript.Instance.LoseGame();
    }
}
