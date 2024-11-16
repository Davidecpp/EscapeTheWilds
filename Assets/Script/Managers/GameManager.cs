using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }
    
    // Reference to the main canvas for UI elements
    public Canvas mainCanvas;
    
    // Game state variables
    public bool inGame; // Tracks if the game is currently in progress
    public bool gameEnded;  // Tracks if the game has ended 
    public int currentScene = 0;  // Stores the current scene index
    public bool victory; // Flag for victory
    
    //Modes
    public bool arenaMode;
    public bool raceMode;
    
    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Pause the game and makes the pointer visible
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; 
        Time.timeScale = 0;
        inGame = false; // Update the game state to indicate it's paused
    }
    
    // Resume the game and hides the pointer
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false; 
        Time.timeScale = 1;
        inGame = true;  // Update the game state to indicate it's active
    }
    
    // Triggered when player's health <= 0
    public void GameOver()
    {
        PauseGame();
        gameEnded = true; // Set the flag to indicate that the game has ended
    }
    
    // Restart game
    public void RestartGame()
    {
        StartCoroutine(RestartAfterDelay(0.1f));
    }
    
    // Restarts the game after a short delay to avoid potential issues
    private IEnumerator RestartAfterDelay(float delay)
    {
        // Wait for a short period of time before restarting to ensure no conflicts
        yield return new WaitForSecondsRealtime(delay); 
        
        gameEnded = false; // Reset gameEnded flag
        inGame = true; // Set the game back to active state
        Time.timeScale = 1; // Ensure time flows normally
        
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        ResumeGame();
    }
}