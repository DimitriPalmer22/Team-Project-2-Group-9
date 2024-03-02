using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateManager : MonoBehaviour
{
    public static ButtonStateManager Instance { get; private set; }

    private static bool _masterButtonFilled;
    private static bool _noviceButtonFilled;

    private void Start()
    {
        if (Instance == null || Instance == this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Awake()
    {
        if (Instance == null || Instance == this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public static void ToggleMasterButtonState()
    {
        // Toggle the state
        _masterButtonFilled = !_masterButtonFilled;
    }

    public static void ToggleNoviceButtonState()
    {
        // Toggle the state
        _noviceButtonFilled = !_noviceButtonFilled;
    }

    public static bool IsMasterButtonFilled => _masterButtonFilled;

    public static bool IsNoviceButtonFilled => _noviceButtonFilled;

    public void ExitGame()
    {
        // Debug log "Exiting the app"
        Debug.Log("Exiting the app");

        // Exit the game
        Application.Quit();
    }
}