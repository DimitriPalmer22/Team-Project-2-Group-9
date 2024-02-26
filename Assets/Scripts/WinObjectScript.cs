using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObjectScript : MonoBehaviour
{

    #region Fields
    
    
    
    #endregion Fields
    
    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion Unity Methods
    

    #region Methods
    
    public void PickUp()
    {
        // Debug log that the player has picked up the object
        Debug.Log("You have picked up the object!");
        
        // Win the game
        GlobalScript.Instance.WinGame();
        
        // Destroy the win object
        Destroy(gameObject);
    }
    
    #endregion Methods

}
