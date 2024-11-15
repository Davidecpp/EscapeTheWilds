using System.Collections;
using UnityEngine;

public class Pepper : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    private PlayerStats _playerStats;

    private void Start()
    {
        // Find the PlayerStats component in the scene and assign it to _playerStats
        _playerStats = FindObjectOfType<PlayerStats>();
        
        // Check if _playerStats is null and log an error if it is
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats component not found in the scene!");
        }
    }

    // Object interaction
    // Makes bullets inflammable for a short period of time
    public bool Interact(Interactor interactor)
    {
        // Check if _playerStats is not null before starting the coroutine
        if (_playerStats != null)
        {
            StartCoroutine(FlameOn(5.0f));
        }
        else
        {
            _playerStats = FindObjectOfType<PlayerStats>();
        }

        if (shouldDisappear)
        {
            // Destroy the game object if shouldDisappear is true
            Destroy(gameObject);
        }
        return true;
    }

    public IEnumerator FlameOn(float seconds)
    {
        // Check if _playerStats is not null before accessing it
        if (_playerStats != null)
        {
            // Makes player heated for seconds time
            _playerStats.heated = true;
            yield return new WaitForSeconds(seconds);
            _playerStats.heated = false;
        }
        else
        {
            Debug.LogError("PlayerStats not assigned!");
        }
    }
}