using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private PlayerStats _playerStats;
    private GameManager _gameManager;
    
    [SerializeField] private RawImage redFlashImage;
    private float _flashDuration = 0.2f;
    
    // Life
    public RawImage heartPrefab; 
    public Transform heartsContainer;
    private List<RawImage> _hearts = new List<RawImage>();
    
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        _gameManager = FindObjectOfType<GameManager>();
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Updates player's life
    public void UpdateHearts()
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
