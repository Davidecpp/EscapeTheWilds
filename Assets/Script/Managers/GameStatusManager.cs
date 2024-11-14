using UnityEngine;

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

    // This method is used to restart the game
    public void RestartGame()
    {
        // Call the RestartGame method from the GameManager to reload the scene and restart the game
        GameManager.Instance.RestartGame();
    }
}