using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOver, boost, win;
    public GameObject menu;
    public GameObject skills, canvas, shop;
    
    // Vita
    public RawImage heartPrefab; 
    public Transform heartsContainer;
    private List<RawImage> _hearts = new List<RawImage>();
    
    public static GameManager Instance { get; private set; }
    
    // Effetto boost
    private static TextMeshProUGUI BoostText { get; set; } 
    [SerializeField] private TextMeshProUGUI boostText;
    
    // Effetto danno
    [SerializeField] private RawImage redFlashImage;
    [SerializeField] private float flashDuration = 0.1f;

    [SerializeField] private int winCondition;
    
    // Giri
    public int laps = 0;
    public int totLaps;
    public TMP_Text lapsTxt;
    
    private PlayerStats _playerStats;
    private Inventory _inventory;
    
    // Condizioni
    public bool invincible = false;
    public bool healing = false;
    public bool heated = false;
    public int coins = 0;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BoostText = boostText; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats not found in the scene.");
            return;
        }

        _inventory = FindObjectOfType<Inventory>();
        if (_inventory == null)
        {
            Debug.LogError("Inventory not found in the scene.");
        }

        InitializeHearts();
        UpdateHearts();
        SetStartActivation();
    }


    void Update()
    {
        CheckWin();
        PopUpWindows();
        UpdateLap();

        if (heated)
        {
            StartCoroutine(FlameOff(5.0f));
        }
        
    }
    
    // Aggiorna giro
    private void UpdateLap()
    {
        lapsTxt.text = laps + "/" + totLaps;
    }
    
    // Inizializzazioni
    private void SetStartActivation()
    {
        gameOver.SetActive(false);
        win.SetActive(false);
        shop.SetActive(false);
        BoostText.gameObject.SetActive(false);
        if (redFlashImage != null)
        {
            redFlashImage.gameObject.SetActive(false); 
        }
    }
    
    // Finestre apribili
    private void PopUpWindows()
    {
        // TAB per aprire miglioramento skills
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (skills != null)
            {
                skills.SetActive(!skills.activeSelf);
            }
        }
        // M per aprire negozio
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (shop != null)
            {
                bool isShopOpen = shop.activeSelf;
                shop.SetActive(!isShopOpen);
                
                // Se negozio aperto, chiudo e riprendi il gioco
                if (isShopOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked; 
                    Cursor.visible = false; 
                    Time.timeScale = 1;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true; 
                    Time.timeScale = 0;
                }
            }
        }

    }
    
    // inizializza vita personaggio
    private void InitializeHearts()
    {
        for (int i = 0; i < _playerStats.health; i++)
        {
            RawImage heart = Instantiate(heartPrefab, heartsContainer);
            _hearts.Add(heart);
        }
    }
    
    // Aggiorna vita personaggio
    private void UpdateHearts()
    {
        while (_hearts.Count < _playerStats.health)
        {
            RawImage heart = Instantiate(heartPrefab, heartsContainer);
            _hearts.Add(heart);
        }

        while (_hearts.Count > _playerStats.health)
        {
            RawImage heart = _hearts[_hearts.Count - 1];
            _hearts.Remove(heart);
            Destroy(heart.gameObject);
        }
        
        for (int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].gameObject.SetActive(i < _playerStats.health);
        }
        
        if (_playerStats.health <= 0)
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            gameOver.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void CheckWin()
    {
        if (_inventory != null && _inventory.GetStrawberryCount() >= winCondition)
        {
            WinGame();
        }

        if (laps == totLaps)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
        win.SetActive(true);
        //Time.timeScale = 0;
    }
    
    // Diminuisci vita personaggio
    public void DecreaseHealth()
    {
        if (_playerStats.health > 0 && !invincible)
        {
            _playerStats.health--;
            UpdateHearts();
            StartCoroutine(FlashRed());
        }
    }
    
    // Aumenta vita personaggio
    public void IncreaseHealth()
    {
        _playerStats.health++;
        UpdateHearts();
    }
    
    // Ricomincia partita
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Awake();
    }

    public void ShowBoost(float duration)
    {
        if (BoostText != null)
        {
            StartCoroutine(ShowBoostCoroutine(duration));
        }
    }
    
    // Effetto boost
    private IEnumerator ShowBoostCoroutine(float duration)
    {
        BoostText.gameObject.SetActive(true);
        boost.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);

        if (BoostText != null)
        {
            BoostText.gameObject.SetActive(false);
            boost.gameObject.SetActive(false);
        }
    }
    
    // Effetto danno
    private IEnumerator FlashRed()
    {
        if (redFlashImage != null)
        {
            redFlashImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            redFlashImage.gameObject.SetActive(false);
        }
    }

    public IEnumerator FlameOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        heated = false;
        Debug.Log("Finish");
    }
}
