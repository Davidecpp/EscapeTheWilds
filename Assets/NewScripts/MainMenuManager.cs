using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject buttons;
    public GameObject gameModes;
    public GameObject controls;
    public GameObject options;
    public GameObject characterSelection;
    public GameObject escBtn;
    
    public GameObject[] characters;
    private int _activeScene;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseTab(gameModes);
            CloseTab(controls);
            CloseTab(options);
            CloseCharacterSelection();
            CloseControls();
        }
        escBtn.SetActive(!buttons.activeSelf);
    }
    public void Play(int i)
    {
        // 1 ARENA - 2 FOOTBALL - 3 RACE - 4 STORY
        SceneManager.LoadSceneAsync(i);
        GameManager.Instance.ResumeGame();
        GameManager.Instance.mainCanvas.gameObject.SetActive(true);
    }
    
    // Open tab
    public void OpenTab(GameObject gameObject)
    {
        gameObject.SetActive(true);
        buttons.SetActive(false);
    }
    
    // Close tab
    private void CloseTab(GameObject go)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
            buttons.SetActive(true);
        }
    }
    
    // Controls
    public void OpenControls()
    {
        options.SetActive(false);
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        if (controls.activeSelf)
        {
            controls.SetActive(false);
            options.SetActive(true);
        }
    }
    // Character selection menu
    public void OpenCharacterSelection(int i)
    {
        gameModes.SetActive(false);
        characterSelection.SetActive(true);
        _activeScene = i;
    }
    private void CloseCharacterSelection()
    {
        if (characterSelection.activeSelf)
        {
            gameModes.SetActive(true);
            characterSelection.SetActive(false);
        }
    }
    
    // Character selection
    public void SelectCharacter(int characterID)
    {
        // Saves selected character's ID
        PlayerPrefs.SetInt("SelectedCharacter", characterID);
        Debug.Log("Selected: " + characterID);

        // Change scene after choosing a character
        SceneManager.LoadScene(_activeScene);
        GameManager.Instance.ResumeGame();
        GameManager.Instance.mainCanvas.gameObject.SetActive(true);
    }
    
    // Quit game
    public void Quit()
    {
        Application.Quit();
    }
}
