using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game objects
    public GameObject gameOver, boost, win;
    public GameObject menu;
    public GameObject skills, canvas, shop;
    
    // Life
    public RawImage heartPrefab; 
    public Transform heartsContainer;
    private List<RawImage> _hearts = new List<RawImage>();
    
    public static GameManager Instance { get; private set; }
    
    // Damage effect
    [SerializeField] private RawImage redFlashImage;
    [SerializeField] private float flashDuration = 0.1f;

    [SerializeField] private int winCondition;
    
    // Laps
    public int laps = 0;
    public int totLaps;
    public TMP_Text lapsTxt;
    
    private PlayerStats _playerStats;
    private Inventory _inventory;
    private CanvasManager _canvasManager;
    
    // Conditions
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
            return;
        }

        _canvasManager = FindObjectOfType<CanvasManager>();
        UpdateHearts();
    }
    
    // Pause the game and makes the pointer visible
    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        Time.timeScale = 0;
    }
    
    // Resume the game and hides the pointer
    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        Time.timeScale = 1;
    }
    
    // Updates player's life
    private void UpdateHearts()
    {
        // Create an heart image for how much health the player has
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
        GameOver();
    }
    // Game over
    public void GameOver()
    {
        if (_playerStats.health <= 0)
        {
            PauseGame();
            gameOver.SetActive(true);
        }
        else
        {
            gameOver.SetActive(false);
            ResumeGame();
        }
    }

    void Update()
    {
        CheckWin();
        TabsOpener();
        UpdateLap();

        if (heated)
        {
            StartCoroutine(FlameOff(5.0f));
        }
    }
    private void UpdateLap()
    {
        lapsTxt.text = laps + "/" + totLaps;
    }
    
    // Open tabs pressing keys
    private void TabsOpener()
    {
        bool tabPressed = Keyboard.current.tabKey.wasPressedThisFrame;
        bool mPressed = Keyboard.current.mKey.wasPressedThisFrame;
        OpenTab(tabPressed, skills);
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
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
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
    }
    
    // Decrease player's life
    public void DecreaseHealth()
    {
        if (_playerStats.health > 0 && !invincible)
        {
            _playerStats.health--;
            UpdateHearts();
            StartCoroutine(_canvasManager.FlashRed());
        }
    }
    
    // Increase player's life
    public void IncreaseHealth()
    {
        _playerStats.health++;
        UpdateHearts();
    }
    
    // Ricomincia partita
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Awake();
    }
    
    // Damage effect (makes the screen red)
    
    public IEnumerator FlameOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        heated = false;
        Debug.Log("Finish");
    }
}
//273