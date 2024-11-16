using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // UI GameObjects references
    [Header("UI Screens")]
    [SerializeField] private GameObject buttons;                // Main menu buttons
    [SerializeField] private GameObject gameModes;               // Game modes selection menu
    [SerializeField] private GameObject controls;                // Controls settings menu
    [SerializeField] private GameObject options;                 // Options menu
    [SerializeField] private GameObject characterSelection;      // Character selection menu
    [SerializeField] private GameObject escBtn;                  // Escape button visibility toggle
    
    [SerializeField] private GameObject[] characters;            // List of character GameObjects
    private int _activeScene;                                    // Store the active scene index

    // Initialize the GameManager's scene at the start
    private void Start()
    {
        GameManager.Instance.currentScene = 0; // Default scene set to 0 (Main Menu)
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the escape key was pressed
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseAllTabs();  // Close all open menus if escape is pressed
        }
        
        // Toggle visibility of the escape button based on the main menu visibility
        escBtn.SetActive(!buttons.activeSelf);
    }

    // Start the game with the selected scene (e.g., Arena, Football, Race, or Story)
    public void Play(int sceneIndex)
    {
        // Set the current scene based on the selection (arena, football, etc.)
        GameManager.Instance.currentScene = sceneIndex;

        // Load the selected scene asynchronously
        SceneManager.LoadSceneAsync(sceneIndex);

        // Resume the game and show the main UI canvas
        GameManager.Instance.ResumeGame();
        GameManager.Instance.mainCanvas.gameObject.SetActive(true);
    }
    
    // Open a specific menu tab (e.g., game modes, options, or controls)
    public void OpenTab(GameObject tab)
    {
        // Activate the tab and deactivate the main menu buttons
        tab.SetActive(true);
        buttons.SetActive(false);
    }
    
    // Close a specific menu tab and return to the main menu buttons
    private void CloseTab(GameObject tab)
    {
        if (tab.activeSelf) // Only deactivate if the tab is active
        {
            tab.SetActive(false);
            buttons.SetActive(true); // Re-enable the main menu buttons
        }
    }
    
    // Close all tabs (game modes, options, controls, and character selection)
    private void CloseAllTabs()
    {
        CloseTab(gameModes);           // Close the game modes tab
        CloseTab(controls);            // Close the controls settings tab
        CloseTab(options);             // Close the options tab
        CloseCharacterSelection();     // Close the character selection menu
        CloseControls();               // Close controls if open
    }

    // Open the controls menu (typically from the options menu)
    public void OpenControls()
    {
        // Hide options and show controls settings
        options.SetActive(false);
        controls.SetActive(true);
    }

    // Close the controls menu and return to the options menu
    public void CloseControls()
    {
        if (controls.activeSelf) // Check if controls are currently visible
        {
            controls.SetActive(false);
            options.SetActive(true); // Show options menu again
        }
    }

    // Open the character selection menu for a specific game mode
    public void OpenCharacterSelection(int sceneIndex)
    {
        // Hide the game modes menu and show the character selection menu
        gameModes.SetActive(false);
        characterSelection.SetActive(true);
        
        _activeScene = sceneIndex;  // Store the selected scene for later use (e.g., arena mode)

        // Set the GameManager's arena mode flag if the selected mode is Arena
        if (_activeScene == 1) // Arena mode
        {
            GameManager.Instance.arenaMode = true;
        }

        if (_activeScene == 3)
        {
            GameManager.Instance.raceMode = true;
        }
    }

    // Close the character selection menu and return to the game modes menu
    private void CloseCharacterSelection()
    {
        if (characterSelection.activeSelf) // Check if character selection is active
        {
            gameModes.SetActive(true);  // Show the game modes menu again
            characterSelection.SetActive(false); // Hide the character selection menu
        }
    }
    
    // Select a character for the game and start the selected scene
    public void SelectCharacter(int characterID)
    {
        // Save the selected character's ID for later use (e.g., in PlayerPrefs)
        PlayerPrefs.SetInt("SelectedCharacter", characterID);

        // Change the active scene based on the previously set scene index
        GameManager.Instance.currentScene = _activeScene;
        SceneManager.LoadScene(_activeScene); // Load the scene associated with the selected mode

        // Resume the game and display the main canvas UI
        GameManager.Instance.ResumeGame();
        GameManager.Instance.mainCanvas.gameObject.SetActive(true);
    }
    
    // Quit the game (used for both editor and final build)
    public void Quit()
    {
        Application.Quit(); // Close the application
    }
}
