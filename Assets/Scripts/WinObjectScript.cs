using UnityEngine;

public class WinObjectScript : MonoBehaviour
{

    #region Methods
    
    public void PickUp()
    {
        // Win the game
        GlobalScript.Instance.WinGame();
        
        // Destroy the win object
        Destroy(gameObject);
    }
    
    #endregion Methods

}
