using UnityEngine;

public class GameStatusManager : MonoBehaviour
{
    public GameObject gameOverObj;
    public GameObject winObj;

    // Update is called once per frame
    void Update()
    {
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        if (GameManager.Instance.gameEnded)
        {
            gameOverObj.SetActive(true);
        }
        else
        {
            gameOverObj.SetActive(false);
        }
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
