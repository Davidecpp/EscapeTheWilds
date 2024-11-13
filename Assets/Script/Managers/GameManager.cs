using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance { get; private set; }
    public Canvas mainCanvas;
    
    // Conditions
    public bool invincible = false;
    public bool healing = false;

    public bool inGame;
    public bool gameEnded;
    public int currentScene = 0;
    
    //Modes
    public bool arenaMode;
    
    private void Awake()
    {
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        Time.timeScale = 0;
        inGame = false;
    }
    
    // Resume the game and hides the pointer
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        Time.timeScale = 1;
        inGame = true;
    }
    
    // Game over if player's health <= 0
    public void GameOver()
    {
        PauseGame();
        gameEnded = true;
    }
    
    void Update()
    {
        Debug.Log("inGame = "+inGame);
        Debug.Log("gameEnded = "+gameEnded);
    }
    
    // Restart game
    public void RestartGame()
    {
        StartCoroutine(RestartAfterDelay(0.1f));
    }
    
    // Delay restart for avoiding problems
    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); 
        
        gameEnded = false;
        inGame = true;
        Time.timeScale = 1; 

        Debug.Log("Before LoadScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Debug.Log("After LoadScene");
        ResumeGame();
        Debug.Log("Game restarted");
    }
}