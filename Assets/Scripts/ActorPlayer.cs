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
        // TODO: Delete later
        
        if (Input.GetKeyDown(KeyCode.U))
            ChangeHealth(-1);
        
        if (Input.GetKeyDown(KeyCode.I))
            ChangeHealth(1);
    }

    protected override void Die()
    {
        GlobalScript.Instance.LoseGame();
    }
}
