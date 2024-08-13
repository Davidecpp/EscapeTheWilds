using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public void SelectHorse()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Horse");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SelectTiger()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Tiger");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SelectDeer()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Deer");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}