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
    [SerializeField] private RawImage redFlashImage;
    private float _flashDuration = 0.2f;
    
    // Life
    public RawImage heartPrefab; 
    public Transform heartsContainer;
    private List<RawImage> _hearts = new List<RawImage>();
    public GameObject maxLifeTxt;
    
    // Laps
    public int laps = 0;
    public int totLaps;
    public TMP_Text lapsTxt;
    
    [SerializeField] private Image keyImage;
    public Slider sprintSlider;
    
    // Ability Images
    public Image currentAbilityImg;
    private PlayerAbility _ability;
    public Sprite venomImg;
    public Sprite dashImg;
    public Sprite jumpImg;
    public bool isAbiliting = false;
    public Slider abilityCooldownSlider;
    
    public GameObject shop;
    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
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
    
    // Update is called once per frame
    void Update()
    {
        if (_playerStats == null)
        {
            _playerStats = FindObjectOfType<PlayerStats>();
            if (_playerStats != null)
            {
                SetPlayerReference(_playerStats);
            }
            else
            {
                return;
            }
        }
        if (_inventory == null)
        {
            _inventory = FindObjectOfType<Inventory>();
            if (_inventory != null)
            {
                SetInventoryReference(_inventory);
            }
            else
            {
                return;
            }
        }
        if (_ability == null)
        {
            _ability = FindObjectOfType<PlayerAbility>();
            if (_ability != null)
            {
                SetAbilityReference(_ability);
            }
            else
            {
                return;
            }
        }
        TabsOpener();
        SetAbilityImg();
        UpdateLap();
        SetImageAlpha(keyImage, _inventory.hasKey ? 1f : 0.3f);
    }
    public void SetPlayerReference(PlayerStats player)
    {
        //SaveSystem.LoadPlayer(player);
        _playerStats = player;
    }
    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }
    public void SetAbilityReference(PlayerAbility playerAbility)
    {
        _ability = playerAbility;
    }
    
    // Update abilty slider
    public void UpdateAbilityCooldown(float currentCooldown, float maxCooldown)
    {
        abilityCooldownSlider.maxValue = maxCooldown;
        abilityCooldownSlider.value = maxCooldown - currentCooldown;
    }

    // Ability Images
    private void SetAbilityImg()
    {
        if (_ability != null)
        {
            if (_ability.characterName.Equals("Deer"))
            {
                currentAbilityImg.sprite = dashImg;
            }
            if (_ability.characterName.Equals("Snake"))
            {
                currentAbilityImg.sprite = venomImg;
            }
            if (_ability.characterName.Equals("Rat"))
            {
                currentAbilityImg.sprite = jumpImg;
            }
        }
        SetImageAlpha(currentAbilityImg, isAbiliting ? 0.5f : 1f);
    }
        
    // Open tabs pressing keys
    private void TabsOpener()
    {
        bool mPressed = Keyboard.current.mKey.wasPressedThisFrame;
        OpenTab(mPressed, shop);
    }
    
    // General method for opening tabs
    private void OpenTab(bool key, GameObject go)
    {
        if (key)
        {
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
    }
    
    // Change image alpha
    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
    
    // Updates player's life
    public void UpdateHearts()
    {
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats not assigned!");
            return;
        }
        if (heartsContainer != null && heartPrefab != null)
        {
            while (_hearts.Count < _playerStats.GetMaxHealth())
            {
                RawImage heart = Instantiate(heartPrefab, heartsContainer);
                _hearts.Add(heart);
            }

            for (int i = 0; i < _hearts.Count; i++)
            {
                _hearts[i].gameObject.SetActive(i < _playerStats.GetHealth());
            }
        }
        maxLifeTxt.SetActive(_playerStats.GetHealth() == _playerStats.GetMaxHealth());
    }
    
    private void UpdateLap()
    {
        if (lapsTxt != null)
        {
            lapsTxt.text = laps + "/" + totLaps;
        }
    }
    
    // Damage effect (makes the screen red)
    public IEnumerator FlashRed()
    {
        if (redFlashImage != null)
        {
            redFlashImage.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(_flashDuration);
            redFlashImage.gameObject.SetActive(false);
        }
    }
}