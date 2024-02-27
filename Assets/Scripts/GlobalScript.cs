using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    public static GlobalScript Instance { get; private set; }

    #region Fields

    // Boolean to determine if the game is over
    private bool _isGameOver;
    
    [Header("Audio")]

    // Audio clip variable for background music
    [SerializeField] private AudioClip backgroundMusic;
    
    // Audio clip variable for win music
    [SerializeField] private AudioClip winMusic;
    
    // Audio clip variable for lose music
    [SerializeField] private AudioClip loseMusic;

    // Audio source variable for music
    private AudioSource _musicSource;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #endregion Unity Methods

    #region Methods

    private void EndGame()
    {
        // If the game is already over, return
        if (_isGameOver)
            return;
        
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

        // TODO: Display win screen

    }

    public void LoseGame()
    {
        // End the game
        EndGame();
        
        // Play lose music
        PlayMusic(loseMusic);
        
        // TODO: Display lose screen
        
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

    #endregion
    
}
