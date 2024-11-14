using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // UI References
    [Header("UI References")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject options;
    [SerializeField] public GameObject menu;
    [SerializeField] private GameObject stats;

    private GameManager _gameManager;
    private CanvasManager _canvas;
    private MissionManager _missionManager;
    private MissionUI _missionUI;

    private bool isMenuActive;

    private void Start()
    {
        _gameManager = GameManager.Instance;  // Direct reference to singleton instance
        _canvas = FindObjectOfType<CanvasManager>();
        _missionManager = FindObjectOfType<MissionManager>();
        _missionUI = FindObjectOfType<MissionUI>();
    }

    private void Update()
    {
        // Handle ESC key for toggling the menu
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (isMenuActive)
        {
            CloseAllTabs();
        }
        else if (!_canvas.shop.activeSelf)
        {
            menu.SetActive(true);  // Show the menu
            _gameManager.PauseGame();  // Pause the game
            isMenuActive = true;
        }
    }

    // Close all open tabs and hide the menu
    private void CloseAllTabs()
    {
        CloseTab(options);
        CloseTab(controls);
        CloseTab(stats);
        menu.SetActive(false); // Hide menu
        _gameManager.ResumeGame(); // Resume game
        isMenuActive = false;
    }

    // Open a specific tab and hide the main buttons
    public void OpenTab(GameObject tab)
    {
        tab.SetActive(true);
        buttons.SetActive(false);
    }

    // Close a specific tab and show the main buttons
    private void CloseTab(GameObject tab)
    {
        if (tab.activeSelf)
        {
            tab.SetActive(false);
            buttons.SetActive(true);
        }
    }

    // Resume the game by hiding the menu
    public void ResumeGame()
    {
        _gameManager.ResumeGame();
        menu.SetActive(false);
        isMenuActive = false;
    }

    // Exit the game and load the main menu scene
    public void ExitGame()
    {
        // Reset mission data and change the scene
        SceneManager.LoadSceneAsync(0);
        _gameManager.arenaMode = false;
        menu.SetActive(false);
        _missionManager.ResetMissionStatus();
        _missionUI._nextScene = 6;
    }

    // Load player stats
    public void Load()
    {
        FindObjectOfType<PlayerStats>().LoadStats();
    }
}
