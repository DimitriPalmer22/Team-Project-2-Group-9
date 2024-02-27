using UnityEngine;

public class WinObjectCollectorScript : MonoBehaviour
{
    #region Fields

    // The win object that the player is currently looking at
    private WinObjectScript _winObject;

    // The camera
    private Camera _camera;

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        // Set the camera to the main camera
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    #endregion Unity Methods

    #region Methods

    private void UpdateInput()
    {
        // If the player presses the pickup spell button, pick up the win object
        if (Input.GetKeyDown(SpellCastScript.PickupSpellKey))
        {
            // Debug log that the player is attempting to pick up a win object
            Debug.Log("Attempting to pick up win object!");
            
            // Determine which win object the player is looking at
            DetermineLookedAtWinObject();
            
            // If the player is looking at a win object, pick it up
            if (_winObject != null)
                _winObject.PickUp();
        }
    }

    private void DetermineLookedAtWinObject()
    {
        // reset the win object the player is looking at
        _winObject = null;

        // Get the camera's transform
        Transform cameraTransform = _camera.transform;
        
        // TODO: Change the distance that the player can pick up the win object from
        
        // TODO: Consolidate the raycasts into 1 big raycast that checks for both spell pickups and win objects
        
        // RayCast to determine if the player is looking at a GameObject tagged "Spell Pickup"
        var hitAGameObject = Physics.Raycast(
            cameraTransform.position, 
            cameraTransform.forward, out var hit, SpellCastScript.PickupDistance);
        
        // If the player is not looking at a GameObject, return
        if (!hitAGameObject) 
            return;
        
        // If the player is not looking at a win object, return
        if (!hit.collider.CompareTag("Win Object")) 
            return;
        
        // Set the _winObject to the WinObjectScript of the object the player is looking at
        _winObject = hit.collider.GetComponent<WinObjectScript>();
    }

    #endregion Methods
}