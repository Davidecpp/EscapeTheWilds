using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private PlayerStats _playerStats;
    private GameManager _gameManager;
    
    // Damage
    [SerializeField] private RawImage redFlashImage;
    private float _flashDuration = 0.2f;
    
    // Life
    public RawImage heartPrefab; 
    public Transform heartsContainer;
    private List<RawImage> _hearts = new List<RawImage>();
    public GameObject maxLifeTxt;
    
    // Ability Images
    public Image currentAbilityImg;
    private PlayerAbility _ability;
    public Sprite venomImg;
    public Sprite dashImg;
    public Sprite jumpImg;
    public bool isAbiliting = false;
    
    public GameObject shop;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        _gameManager = FindObjectOfType<GameManager>();
        _ability = FindObjectOfType<PlayerAbility>();
        UpdateHearts();
        _gameManager.ResumeGame();
    }
    // Update is called once per frame
    void Update()
    {
        TabsOpener();
        SetAbilityImg();
    }
    // Ability Images
    private void SetAbilityImg()
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

        SetImageAlpha(currentAbilityImg, isAbiliting ? 0.5f : 1f);
    }
        
    // Open tabs pressing keys
    private void TabsOpener()
    {
        //bool tabPressed = Keyboard.current.tabKey.wasPressedThisFrame;
        bool mPressed = Keyboard.current.mKey.wasPressedThisFrame;
        //OpenTab(tabPressed, skills);
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
                    _gameManager.PauseGame();
                    if (Keyboard.current.escapeKey.wasPressedThisFrame)
                    {
                        go.SetActive(false);
                        _gameManager.ResumeGame();
                    }
                }
                else
                {
                    _gameManager.ResumeGame();
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
        // Create an heart image for how much health the player has
        while (_hearts.Count < _playerStats.GetHealth())
        {
            RawImage heart = Instantiate(heartPrefab, heartsContainer);
            _hearts.Add(heart);
        }

        while (_hearts.Count > _playerStats.GetHealth())
        {
            RawImage heart = _hearts[_hearts.Count - 1];
            _hearts.Remove(heart);
            Destroy(heart.gameObject);
        }
        
        for (int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].gameObject.SetActive(i < _playerStats.GetHealth());
        }

        if (_playerStats.GetHealth() == _playerStats.maxHealth)
        {
            Debug.Log("Max life");
            maxLifeTxt.SetActive(true);
        }
        else
        {
            maxLifeTxt.SetActive(false);
        }
        _gameManager.GameOver();
    }
    
    // Damage effect (makes the screen red)
    public IEnumerator FlashRed()
    {
        if (redFlashImage != null)
        {
            redFlashImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(_flashDuration);
            redFlashImage.gameObject.SetActive(false);
        }
    }
}