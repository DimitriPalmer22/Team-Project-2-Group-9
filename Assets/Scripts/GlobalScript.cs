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

    [Header("UI")] [SerializeField] private GameObject winScreen;

    [SerializeField] private GameObject loseScreen;

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
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = _isGameOver || _isGamePaused;
        if (Cursor.visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion Unity Methods

    #region Methods

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
        // Time.timeScale = 0;
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
    }

    public void ExitGame()
    {
        // Debug log "Exiting the app"
        Debug.Log("Exiting the app");
        
        // Exit the app
        Application.Quit();
    }

    #endregion
}