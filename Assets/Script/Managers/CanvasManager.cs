using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // References to player stats and inventory
    private PlayerStats _playerStats;
    private Inventory _inventory;
    
    // Counters for various resources like strawberries, coins, and bullets
    [SerializeField] public Inventory.ResourceCounter strawberryCounter;
    [SerializeField] public Inventory.ResourceCounter coinCounter;
    [SerializeField] public Inventory.ResourceCounter bulletCounter;
    
    // Damage effect setup
    [SerializeField] private RawImage redFlashImage; // Image for the damage flash effect
    private float _flashDuration = 0.2f; // Duration of the damage effect

    public GameObject boostAnimation; // Animation for speed boost
    
    [Header("Health")]
    [SerializeField] private RawImage heartPrefab;  // Heart object used for health display
    [SerializeField] private Transform heartsContainer; // Container to hold the heart objects
    private List<RawImage> _hearts = new List<RawImage>(); // List to manage hearts
    [SerializeField] private GameObject maxLifeTxt; // Text to display when health is full
    
    // Lap tracking
    public int laps = 0; // Current lap count
    public int totLaps; // Total laps
    [SerializeField] private TMP_Text lapsTxt; // Lap text display
    
    [SerializeField] private Image keyImage; // Image for the key display
    [SerializeField] public Slider sprintSlider; // Slider to display sprint status
    
    [Header("Ability Images")]
    [SerializeField] private Sprite venomImg; // Sprite for venom ability
    [SerializeField] private Sprite dashImg; // Sprite for dash ability
    [SerializeField] private Sprite jumpImg; // Sprite for jump ability
    [SerializeField] private Image currentAbilityImg; // Image to display current ability
    private PlayerAbility _ability; // Reference to player abilities
    public bool isAbiliting; // Flag to track if the ability is active
    [SerializeField] private Slider abilityCooldownSlider; // Slider to display ability cooldown
    
    // Shop UI setup
    [SerializeField] public GameObject shop; // Shop UI object
    public static CanvasManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of CanvasManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the CanvasManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    
    private void Update()
    {
        // Assign references if they are null
        FindReferences();

        // Return if any reference is still null
        if (_playerStats == null || _inventory == null || _ability == null)
        {
            return;
        }
        
        // Update UI elements
        TabsOpener(); // Handle opening of tabs
        SetAbilityImg(); // Update the ability image based on character
        UpdateLap(); // Update lap text
        SetImageAlpha(keyImage, _inventory.hasKey ? 1f : 0.3f); // Adjust key image visibility
    }
    
    // Set PlayerStats reference
    public void SetPlayerReference(PlayerStats player)
    {
        _playerStats = player;
    }
    
    private void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }

    private void SetAbilityReference(PlayerAbility playerAbility)
    {
        _ability = playerAbility;
    }

    private void FindReferences()
    {
        // Check if the _playerStats reference is null
        if (_playerStats == null)
        {
            // Try to find the PlayerStats object in the scene
            _playerStats = FindObjectOfType<PlayerStats>();
        
            // If found, set the reference
            if (_playerStats != null)
            {
                SetPlayerReference(_playerStats);
            }
            else
            {
                return; // Exit if PlayerStats reference is not found
            }
        }

        // Check if the _inventory reference is null
        if (_inventory == null)
        {
            // Try to find the Inventory object in the scene
            _inventory = FindObjectOfType<Inventory>();
        
            // If found, set the reference
            if (_inventory != null)
            {
                SetInventoryReference(_inventory);
            }
            else
            {
                return; // Exit if Inventory reference is not found
            }
        }

        // Check if the _ability reference is null
        if (_ability == null)
        {
            // Try to find the PlayerAbility object in the scene
            _ability = FindObjectOfType<PlayerAbility>();
        
            // If found, set the reference
            if (_ability != null)
            {
                SetAbilityReference(_ability);
            }
            else
            {
                return; // Exit if PlayerAbility reference is not found
            }
        }
    }

    // Update the ability cooldown UI
    public void UpdateAbilityCooldown(float currentCooldown, float maxCooldown)
    {
        abilityCooldownSlider.maxValue = maxCooldown; // Set maximum value for the cooldown slider
        abilityCooldownSlider.value = maxCooldown - currentCooldown; // Set current cooldown value
    }

    private void SetAbilityImg()
    {
        // Set the ability image based on the character's name
        if (_ability != null)
        {
            switch (_ability.characterType)
            {
                case PlayerAbility.CharacterType.Deer:
                    currentAbilityImg.sprite = dashImg; // Set dash ability image for Deer
                    break;
                case PlayerAbility.CharacterType.Snake:
                    currentAbilityImg.sprite = venomImg; // Set venom ability image for Snake
                    break;
                case PlayerAbility.CharacterType.Rat:
                    currentAbilityImg.sprite = jumpImg; // Set jump ability image for Rat
                    break;
            }
        }
        // Adjust the image alpha based on whether the ability is being used
        SetImageAlpha(currentAbilityImg, isAbiliting ? 0.5f : 1f); // Reduce alpha when ability is in use
    }
        
    private void TabsOpener()
    {
        // Open the shop tab when the 'M' key is pressed
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            OpenTab(shop);
        }
    }
    
    private void OpenTab(GameObject go)
    {
        // Toggle the active state of the tab and pause/resume the game accordingly if ESC is pressed
        if (go != null)
        {
            go.SetActive(!go.activeSelf);
            if (go.activeSelf)
            {
                GameManager.Instance.PauseGame(); // Pause the game when a tab is opened
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    go.SetActive(false); // Close tab when ESC is pressed
                    GameManager.Instance.ResumeGame(); // Resume the game
                }
            }
            else
            {
                GameManager.Instance.ResumeGame(); // Resume game if tab is closed
            }
        }
    }
    
    private void SetImageAlpha(Image image, float alpha)
    {
        // Set the alpha value of the image (opacity)
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void UpdateHearts()
    {
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats not assigned!");
            return;
        }
        // Update the heart icons based on the player's health
        if (heartsContainer != null && heartPrefab != null)
        {
            // Instantiate hearts up to the max health
            while (_hearts.Count < _playerStats.GetMaxHealth())
            {
                RawImage heart = Instantiate(heartPrefab, heartsContainer);
                _hearts.Add(heart); // Add heart to the list
            }
            // Update heart visibility based on current health
            for (int i = 0; i < _hearts.Count; i++)
            {
                _hearts[i].gameObject.SetActive(i < _playerStats.GetHealth()); // Show hearts based on current health
            }
        }
        // Show max life text if at full health
        maxLifeTxt.SetActive(_playerStats.GetHealth() == _playerStats.GetMaxHealth());
    }

    private void UpdateLap()
    {
        // Update the lap text
        if (lapsTxt != null)
        {
            lapsTxt.text = laps + "/" + totLaps; // Display current lap / total laps
        }
    }

    public IEnumerator FlashRed()
    {
        // Flash the screen red to indicate damage
        if (redFlashImage != null)
        {
            redFlashImage.gameObject.SetActive(true); // Show red flash
            yield return new WaitForSecondsRealtime(_flashDuration); // Wait for the flash duration
            redFlashImage.gameObject.SetActive(false); // Hide red flash
        }
    }
    
    // Boosts the player's speed for a specified duration
    public void BoostSpeed(float duration)
    {
        var movement = FindObjectOfType<Movement>();
        if (!movement.isBoosted) 
        {
            // Set the player as boosted and activate the boost animation
            movement.isBoosted = true;
            boostAnimation.SetActive(true);
            
            // Start a coroutine to reset the speed after the specified duration
            StartCoroutine(ResetSpeedAfterDelay(duration, movement)); 
        }
    }

    private IEnumerator ResetSpeedAfterDelay(float duration, Movement movement)
    {
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        movement.isBoosted = false; // Reset boosted state
        boostAnimation.SetActive(false); // Hide the boost animation
    }
}
