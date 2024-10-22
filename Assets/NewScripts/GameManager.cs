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
    public GameObject skills, canvas;
    
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
        ResumeGame();
    }
    
    // Pause the game and makes the pointer visible
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        Time.timeScale = 0;
    }
    
    // Resume the game and hides the pointer
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        Time.timeScale = 1;
    }
    
    // Game over
    public void GameOver()
    {
        if (_playerStats.GetHealth() <= 0)
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
        if (_playerStats.GetHealth() > 0 && !invincible)
        {
            _playerStats.ReduceHealth();
            _canvasManager.UpdateHearts();
            StartCoroutine(_canvasManager.FlashRed());
        }
    }
    
    // Increase player's life
    public void IncreaseHealth()
    {
        if (_playerStats.GetHealth() < _playerStats.maxHealth)
        {
            _playerStats.AddHeart();
            _canvasManager.UpdateHearts();
        }
    }
    
    // Restart gamef
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Awake();
    }
    
    public IEnumerator FlameOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        heated = false;
        Debug.Log("Finish");
    }
}
//273