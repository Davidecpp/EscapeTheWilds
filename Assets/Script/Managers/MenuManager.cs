using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // UI References
    [Header("UI References")]
    [SerializeField] private GameObject buttons;            // Main menu buttons
    [SerializeField] private GameObject controls;           // Controls settings menu
    [SerializeField] private GameObject options;            // Options menu
    public GameObject menu;                                 // Main menu object
    [SerializeField] private GameObject stats;              // Stats menu

    private GameManager _gameManager;                       // Reference to the GameManager instance
    private CanvasManager _canvas;                          // Reference to the CanvasManager
    private MissionManager _missionManager;                 // Reference to the MissionManager 
    private MissionUI _missionUI;                           // Reference to the MissionUI
    private bool isMenuActive;                              // Boolean to track if the menu is active

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize the references by finding them in the scene
        _gameManager = GameManager.Instance;  // Direct reference to singleton instance of GameManager
        
        _canvas = FindObjectOfType<CanvasManager>();   // Find CanvasManager in the scene
        _missionManager = FindObjectOfType<MissionManager>();  // Find MissionManager in the scene
        _missionUI = FindObjectOfType<MissionUI>();  // Find MissionUI in the scene


        if (_gameManager.currentScene > 0)  // Check if the current scene is not the main menu
        {
            menu.SetActive(false);  // Hide the menu at the start of the game
            isMenuActive = false;  // Set menu state to inactive
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the ESC key was pressed
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Toggle the menu visibility on ESC key press
            ToggleMenu();
         
        }
    }

    // Toggle the menu's state: open if closed, close if open
    private void ToggleMenu()
    {
        // If menu is active, close all open tabs and hide the menu
        if (isMenuActive)
        {
            CloseAllTabs();
        }
        // If menu is not active, show the menu and pause the game
        else if (!_canvas.shop.activeSelf)
        {
            menu.SetActive(true);  // Activate the menu
            _gameManager.PauseGame();  // Pause the game
            isMenuActive = true;  // Set menu state to active
        }
    }

    // Close all tabs and hide the menu
    private void CloseAllTabs()
    {
        // Close each tab and hide the menu
        CloseTab(options);     // Close options menu
        CloseTab(controls);    // Close controls settings menu
        CloseTab(stats);       // Close stats menu
        menu.SetActive(false); // Hide the menu
        _gameManager.ResumeGame();  // Resume the game
        isMenuActive = false;  // Set menu state to inactive
    }

    // Open a specific tab and hide the main buttons
    public void OpenTab(GameObject tab)
    {
        tab.SetActive(true);  // Activate the selected tab
        buttons.SetActive(false);  // Deactivate the main menu buttons
    }

    // Close a specific tab and show the main buttons again
    private void CloseTab(GameObject tab)
    {
        if (tab.activeSelf)  // Only close the tab if it's currently active
        {
            tab.SetActive(false);  // Deactivate the tab
            buttons.SetActive(true);  // Show the main menu buttons again
        }
    }

    // Resume the game and hide the menu
    public void ResumeGame()
    {
        _gameManager.ResumeGame();  // Resume the game through GameManager
        menu.SetActive(false);  // Hide the menu
        isMenuActive = false;  // Set menu state to inactive
    }

    // Exit the game and load the main menu scene
    public void ExitGame()
    {
        // Reset mission data and change to the main menu scene
        SceneManager.LoadSceneAsync(0);  // Load the main menu scene
        
        _gameManager.arenaMode = false;  // Set the arena mode flag to false
        _gameManager.raceMode = false;  // Set the race mode flag to false
        menu.SetActive(false);  // Hide the menu
        _canvas.gameObject.SetActive(false);  // Hide the canvas


        ResetMissionsAfterExit(); // Reset missions when quitting the game
    }
    
    // Reset mission status and next scene index
    private void ResetMissionsAfterExit()
    {
        // Search for inventory in the scene
        var _inventory = FindObjectOfType<Inventory>();
        if (_inventory != null)
        {
            _inventory.SetCoinCount(0); // Set coin amount to 0
            _inventory.SetStrawberryCount(0);  // Set strawberry amount to 0
        }
        
        _missionManager.ResetMissionStatus();  // Reset the mission status
    }

    // Load player stats from PlayerPrefs or another system
    public void Load()
    {
        var playerStats = FindObjectOfType<PlayerStats>();
        SaveSystem.LoadPlayer(playerStats);
    }
    public void Save()
    {
        var playerStats = FindObjectOfType<PlayerStats>();
        SaveSystem.SavePlayer(playerStats);
    }
}
