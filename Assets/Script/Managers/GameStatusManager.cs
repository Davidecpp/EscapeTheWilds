using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatusManager : MonoBehaviour
{
    // Reference to the game over UI object
    public GameObject gameOverObj;

    // Reference to the win UI object
    public GameObject winObj;

    // Update is called once per frame
    void Update()
    {
        // Check and display the Game Over screen if the game has ended
        ShowGameOverScreen();
        ShowWinScreen();
    }

    // This method controls the visibility of the Game Over screen
    private void ShowGameOverScreen()
    {
        // If the game has ended, activate the game over screen
        if (GameManager.Instance.gameEnded)
        {
            gameOverObj.SetActive(true);
        }
        else
        {
            // Otherwise, deactivate the game over screen
            gameOverObj.SetActive(false);
        }
    }
    private void ShowWinScreen()
    {
        // If the game has ended, activate the game over screen
        if (GameManager.Instance.victory)
        {
            winObj.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        else
        {
            // Otherwise, deactivate the game over screen
            winObj.SetActive(false);
        }
    }

    // This method is used to restart the game
    public void RestartGame()
    {
        // Call the RestartGame method from the GameManager to reload the scene and restart the game
        GameManager.Instance.RestartGame();
    }
    
    // Set victory false and go back to Hub
    public void BackToHub()
    {
        GameManager.Instance.victory = false;
        SceneManager.LoadScene("Scenes/Modes/StoryMode/Hub"); // Load Hub scene
        GameManager.Instance.ResumeGame(); // Resume game after loading scene
    }
}