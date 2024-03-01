using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStateManager : MonoBehaviour
{
    public static ButtonStateManager Instance { get; private set; }

    private const string MasterButtonKey = "MasterButtonFilled";
    private const string NoviceButtonKey = "NoviceButtonFilled";

    public delegate void ButtonStateChange();
    public event ButtonStateChange OnMasterButtonStateChanged;
    public event ButtonStateChange OnNoviceButtonStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadButtonStates();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadButtonStates()
    {
        bool masterFilled = PlayerPrefs.GetInt(MasterButtonKey, 0) == 1;
        bool noviceFilled = PlayerPrefs.GetInt(NoviceButtonKey, 0) == 1;

    }

    public void ToggleMasterButtonState()
    {
        bool currentState = PlayerPrefs.GetInt(MasterButtonKey, 0) == 1;
        bool newState = !currentState;
        PlayerPrefs.SetInt(MasterButtonKey, newState ? 1 : 0);
        PlayerPrefs.Save();

        OnMasterButtonStateChanged?.Invoke();
    }

    public void ToggleNoviceButtonState()
    {
        bool currentState = PlayerPrefs.GetInt(NoviceButtonKey, 0) == 1;
        bool newState = !currentState;
        PlayerPrefs.SetInt(NoviceButtonKey, newState ? 1 : 0);
        PlayerPrefs.Save();

        OnNoviceButtonStateChanged?.Invoke();
    }

    public bool IsMasterButtonFilled()
    {
        return PlayerPrefs.GetInt(MasterButtonKey, 0) == 1;
    }

    public bool IsNoviceButtonFilled()
    {
        return PlayerPrefs.GetInt(NoviceButtonKey, 0) == 1;
    }

    public void ExitGame()
    {
        // Debug log "Exiting the app"
        Debug.Log("Exiting the app");
        
        // Exit the game
        Application.Quit();
    }
}
