using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalScript : MonoBehaviour
{
    public static GlobalScript Instance { get; private set; }

    #region Fields

    // Boolean to determine if the game is over
    private bool _isGameOver;

    // Boolean to determine if the game is paused
    private bool _isGamePaused;

    [Header("Audio")]

    // Audio clip variable for background music
    [SerializeField]
    private AudioClip backgroundMusic;

    // Audio clip variable for win music
    [SerializeField] private AudioClip winMusic;

    // Audio clip variable for lose music
    [SerializeField] private AudioClip loseMusic;

    // Audio source variable for music
    private AudioSource _musicSource;

    [Header("UI")] 
    
    [SerializeField] private GameObject winScreen;

    [SerializeField] private GameObject loseScreen;
    
    [SerializeField] private GameObject pauseScreenParent;

    /// <summary>
    /// The actual pause screen
    /// </summary>
    private GameObject _pauseMenu;

    #endregion Fields


    #region Unity Methods

    private void Awake()
    {
        // Set the instance to this object
        Instance = this;

        // Play background music if the game is not over
        if (!_isGameOver)
            PlayMusic(backgroundMusic);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get audio source component for music
        _musicSource = GetComponent<AudioSource>();

        // Hide win and lose screens
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        // Hide pause screen parent
        pauseScreenParent.SetActive(false);
        
        // hide each of the pause screen parent's children
        for (int i = 0; i < pauseScreenParent.transform.childCount; i++)
        {
            // Get the child at index i
            var child = pauseScreenParent.transform.GetChild(i);
            
            // Hide the child
            child.gameObject.SetActive(false);
        }
        
        // show the pause screen while keeping the parent hidden
        _pauseMenu = pauseScreenParent.transform.Find("PauseMenu").gameObject;
        _pauseMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Determine cursor visibility
        DetermineCursorVisibility();
        
        // Update input
        UpdateInput();
    }

    #endregion Unity Methods

    #region Methods

    private void DetermineCursorVisibility()
    {
        Cursor.visible = _isGameOver || _isGamePaused;
        if (Cursor.visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isGameOver)
        {
            if (_isGamePaused)
                UnpauseGame();
            else
                PauseGame();
        }
    }

    private void EndGame()
    {
        // If the game is already over, return
        if (_isGameOver)
            return;

        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Set the game over boolean to true
        _isGameOver = true;

        // TODO: Disable player input

        // Freeze the game
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        // End the game
        EndGame();

        // Play win music
        PlayMusic(winMusic);

        // Display win screen
        winScreen.SetActive(true);
    }

    public void LoseGame()
    {
        // End the game
        EndGame();

        // Play lose music
        PlayMusic(loseMusic);

        // Display lose screen
        loseScreen.SetActive(true);
    }

    private void PlayMusic(AudioClip music)
    {
        // If there is no music, return
        if (music == null)
            return;

        // Set the music source's clip to the music
        _musicSource.clip = music;

        // Play music
        _musicSource.Play();
    }

    public void ReturnToMainMenu()
    {
        // Debug log "Returning to main menu"
        Debug.Log("Returning to main menu");
        
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
        
        // Reset the time scale
        Time.timeScale = 1;
        
        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitGame()
    {
        // Debug log "Exiting the app"
        Debug.Log("Exiting the app");
        
        // Exit the app
        Application.Quit();
    }

    private void PauseGame()
    {
        Debug.Log("Pausing the game");

        // Pause the game
        Time.timeScale = 0;
        
        // Show pause screen parent
        pauseScreenParent.SetActive(true);
        
        // hide each of the pause screen parent's children
        for (int i = 0; i < pauseScreenParent.transform.childCount; i++)
        {
            // Get the child at index i
            var child = pauseScreenParent.transform.GetChild(i);
            
            // Hide the child
            child.gameObject.SetActive(false);
        }
        
        // show the pause screen
        _pauseMenu.SetActive(true);
        
        _isGamePaused = true;
    }
    
    public void UnpauseGame()
    {
        Debug.Log("Unpausing the game");
        
        // Unpause the game
        Time.timeScale = 1;
        
        // Hide the pause screen
        pauseScreenParent.SetActive(false);

        _isGamePaused = false;
    }

    #endregion
}