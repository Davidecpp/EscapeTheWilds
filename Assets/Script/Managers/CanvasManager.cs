using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // References
    private PlayerStats _playerStats;
    private Inventory _inventory;
    
    // Resources counters
    [SerializeField] public Inventory.ResourceCounter strawberryCounter;
    [SerializeField] public Inventory.ResourceCounter coinCounter;
    [SerializeField] public Inventory.ResourceCounter bulletCounter;
    
    // Damage
    [SerializeField] private RawImage redFlashImage; // damage image
    private float _flashDuration = 0.2f; // damage effect duration
    
    [Header("Health")]
    [SerializeField] private RawImage heartPrefab;  // heart object
    [SerializeField] private Transform heartsContainer; // hearts container
    private List<RawImage> _hearts = new List<RawImage>();
    [SerializeField] private GameObject maxLifeTxt;
    
    // Laps
    public int laps = 0;
    public int totLaps;
    [SerializeField] private TMP_Text lapsTxt;
    
    [SerializeField] private Image keyImage;
    [SerializeField] public Slider sprintSlider;
    
    [Header("Ability Images")]
    [SerializeField] private Sprite venomImg;
    [SerializeField] private Sprite dashImg;
    [SerializeField] private Sprite jumpImg;
    [SerializeField] private Image currentAbilityImg; // Current ability image based on character
    private PlayerAbility _ability;
    public bool isAbiliting;
    [SerializeField] private Slider abilityCooldownSlider;
    
    // Shop UI
    [SerializeField] public GameObject shop;
    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of CanvasManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        // Assign references if they are null
        _playerStats ??= FindObjectOfType<PlayerStats>();
        _inventory ??= FindObjectOfType<Inventory>();
        _ability ??= FindObjectOfType<PlayerAbility>();
        
        // Return if any reference is still null
        if (_playerStats == null || _inventory == null || _ability == null)
        {
            return;
        }
        
        // Update UI elements
        TabsOpener();
        SetAbilityImg();
        UpdateLap();
        SetImageAlpha(keyImage, _inventory.hasKey ? 1f : 0.3f);
    }
    
    // Set PlayerStats reference
    public void SetPlayerReference(PlayerStats player)
    {
        _playerStats = player;
    }
    // Update the ability cooldown UI
    public void UpdateAbilityCooldown(float currentCooldown, float maxCooldown)
    {
        abilityCooldownSlider.maxValue = maxCooldown;
        abilityCooldownSlider.value = maxCooldown - currentCooldown;
    }

    private void SetAbilityImg()
    {
        // Set the ability image based on the character's name
        if (_ability != null)
        {
            switch (_ability.characterName)
            {
                case "Deer":
                    currentAbilityImg.sprite = dashImg;
                    break;
                case "Snake":
                    currentAbilityImg.sprite = venomImg;
                    break;
                case "Rat":
                    currentAbilityImg.sprite = jumpImg;
                    break;
            }
        }
        // Adjust the image alpha based on whether the ability is being used
        SetImageAlpha(currentAbilityImg, isAbiliting ? 0.5f : 1f);
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
                GameManager.Instance.PauseGame();
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    go.SetActive(false);
                    GameManager.Instance.ResumeGame();
                }
            }
            else
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }
    
    private void SetImageAlpha(Image image, float alpha)
    {
        // Set the alpha value of the image
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
                _hearts.Add(heart);
            }
            // Update heart visibility based on current health
            for (int i = 0; i < _hearts.Count; i++)
            {
                _hearts[i].gameObject.SetActive(i < _playerStats.GetHealth());
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
            lapsTxt.text = laps + "/" + totLaps;
        }
    }
    
    public IEnumerator FlashRed()
    {
        // Flash the screen red to indicate damage
        if (redFlashImage != null)
        {
            redFlashImage.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(_flashDuration);
            redFlashImage.gameObject.SetActive(false);
        }
    }
}
