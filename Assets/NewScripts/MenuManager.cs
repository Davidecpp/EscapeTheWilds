using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject esc;
    public GameObject buttons;
    public GameObject gameModes;
    public GameObject controls;
    public GameObject options;
    public GameObject characterSelection;
    public GameObject menu;
    public GameObject stats;
    
    public GameObject[] characters;
    private int _activeScene;

    private GameManager _gameManager;
    private CanvasManager _canvas;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _canvas = FindObjectOfType<CanvasManager>();
    }
    
    public void ResumeGame()
    {
        _gameManager.ResumeGame();
        menu.SetActive(false);
    }
    private void Update()
    {
        // If ESC is pressed
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Alternate menu state
            if (menu.activeSelf)
            {
                CloseTab(gameModes);
                CloseTab(options);
                CloseTab(controls);
                CloseTab(stats);
                CloseCharacterSelection();
            }
            else if(!_canvas.shop.activeSelf)
            {
                menu.SetActive(true);  
                _gameManager.PauseGame();
            }
        }
        esc.SetActive(!buttons.activeSelf);
    }
    
    // Load MainMenu scene
    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Play(int i)
    {
        // 1 ARENA - 2 FOOTBALL - 3 RACE
        SceneManager.LoadSceneAsync(i);
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
    }
}