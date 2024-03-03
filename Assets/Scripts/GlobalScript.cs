using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalScript : MonoBehaviour
{
    
    private const float TimerLimit = 90;
    
    private const bool UseTimer = false;
    
    public static GlobalScript Instance { get; private set; }

    #region Fields

    // Boolean to determine if the game is over
    private bool _isGameOver;

    // Boolean to determine if the game is paused
    private bool _isGamePaused;

    // The remaining time in the game
    private float _timerRemaining;

    [Header("Audio")]

    // Audio clip variable for background music
    [SerializeField] private AudioClip backgroundMusic;

    // Audio clip variable for win music
    [SerializeField] private AudioClip winMusic;

    // Audio clip variable for lose music
    [SerializeField] private AudioClip loseMusic;

    // Audio source variable for music
    [SerializeField] private AudioSource _musicSource;

    // The volume the music should be reset to
    private float _musicResetVolume;

    [Header("UI")] [SerializeField] private GameObject winScreen;

    [SerializeField] private GameObject loseScreen;

    [SerializeField] private GameObject pauseScreenParent;
    
    [SerializeField] private GameObject inGameUIParent;
    
    [SerializeField] private TMP_Text timerText;

    /// <summary>
    /// The actual pause screen
    /// </summary>
    private GameObject _pauseMenu;

    #endregion Fields
    
    public bool IsGameOver => _isGameOver;

    #region Unity Methods

    private void Awake()
    {
        // Set the instance to this object
        Instance = this;

    }

    // Start is called before the first frame update
    private void Start()
    {
        // set the music reset volume to the music source's volume
        _musicResetVolume = _musicSource.volume;

        // Play background music if the game is not over
        if (!_isGameOver)
            PlayMusic(backgroundMusic);
        
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
        
        // Set the timer remaining to the timer limit
        _timerRemaining = TimerLimit;
    }

    // Update is called once per frame
    private void Update()
    {
        // If the game is over, return
        TickTimer();
        
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
        Cursor.lockState = (Cursor.visible) 
            ? CursorLockMode.None 
            : CursorLockMode.Locked;
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

        // Freeze the game
        Time.timeScale = 0;
        
        // Hide the in-game UI
        inGameUIParent.SetActive(false);
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

        // Set the music source's volume to half of the reset volume if the music is the win music
        _musicSource.volume = (music == winMusic) 
            ? _musicResetVolume / 2 
            : _musicResetVolume;

        // loop the audio source only if the music is the background music
        _musicSource.loop = music == backgroundMusic;

        // Set the music source's clip to the music
        _musicSource.clip = music;

        // Play music
        _musicSource.Play();
    }

    public void ReturnToMainMenu()
    {
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
        
        // Hide the in-game UI
        inGameUIParent.SetActive(false);

        _isGamePaused = true;
    }

    public void UnpauseGame()
    {
        // Unpause the game
        Time.timeScale = 1;

        // Hide the pause screen
        pauseScreenParent.SetActive(false);
        
        // Show the in-game UI
        inGameUIParent.SetActive(true);

        _isGamePaused = false;
    }

    private void TickTimer()
    {
        // If the game is over, return
        if (_isGameOver)
            return;
        
        // hide the timer text if the timer is not being used
        timerText.gameObject.SetActive(UseTimer);
        
        // If the timer is not being used, return
        if (!UseTimer)
            return;
        
        // Decrement the timer remaining
        _timerRemaining -= Time.deltaTime;
        
        // Set the timer text to the timer remaining
        timerText.text = $"Time Remaining: {_timerRemaining:0.00}";
        
        // If the timer remaining is less than or equal to 0, lose the game
        if (_timerRemaining <= 0)
        {
            _timerRemaining = 0;
            
            LoseGame();
        }
    }

    #endregion
}