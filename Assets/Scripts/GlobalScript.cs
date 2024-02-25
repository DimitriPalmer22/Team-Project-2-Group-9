using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    public static GlobalScript Instance { get; private set; }

    #region Fields

    // TODO: Add audio source variable

    #endregion Fields
     

    #region Unity Methods
    
    private void Awake()
    {
        // Set the instance to this object
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Get audio source component
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #endregion Unity Methods

    #region Methods

    private void EndGame()
    {
        // TODO: Disable player input
        
        // Freeze the game
        Time.timeScale = 0;

    }
    
    public void WinGame()
    {
        // End the game
        EndGame();
        
        // TODO: Play win music

        // TODO: Display win screen

    }

    public void LoseGame()
    {
        // End the game
        EndGame();
        
        // TODO: Play win music
        
        // TODO: Display win screen
        
    }

    private void PlayMusic(AudioClip music)
    {
        // TODO: Play music
    }

    #endregion
    
}
