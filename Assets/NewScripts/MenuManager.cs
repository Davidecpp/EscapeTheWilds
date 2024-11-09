using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject buttons;
    public GameObject controls;
    public GameObject options;
    public GameObject menu;
    public GameObject stats;
    
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
                CloseTab(options);
                CloseTab(controls);
                CloseTab(stats);
            }
            else if(!_canvas.shop.activeSelf)
            {
                menu.SetActive(true);  
                _gameManager.PauseGame();
            }
        }
    }
    
    // Load MainMenu scene
    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
        GameManager.Instance.arenaMode = false;
        menu.SetActive(false);
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
}