using System.Collections;
using UnityEngine;

public class Pepper : MonoBehaviour, IInteractible
{
    // Interaction prompt shown when the player can interact with the pepper
    [SerializeField] private string prompt;
    
    // Flag to indicate whether the pepper should disappear after interaction
    [SerializeField] private bool shouldDisappear;
    
    // Interaction prompt property
    public string InteractionPrompt => prompt;

    // Bonus object flag property
    public bool bonusObj { get; private set; } = true;

    // Reference to PlayerStats for accessing player status
    private PlayerStats _playerStats;

    private void Start()
    {
        // Find the PlayerStats component in the scene and assign it to _playerStats
        _playerStats = FindObjectOfType<PlayerStats>();
        
        // Check if _playerStats is null and log an error if it is
        if (_playerStats == null)
        {
            return;
        }
    }

    // Interaction with the pepper: makes bullets inflammable for a short period of time
    public bool Interact(Interactor interactor)
    {
        // Check if _playerStats is not null before starting the coroutine
        if (_playerStats != null)
        {
            StartCoroutine(FlameOn(5.0f)); // Activates flame effect for 5 seconds
        }
        else
        {
            // If PlayerStats is not found, try to find it again
            _playerStats = FindObjectOfType<PlayerStats>();
        }

        // If shouldDisappear is true, destroy the pepper object after interaction
        if (shouldDisappear)
        {
            Destroy(gameObject);
        }

        return true;
    }

    // Coroutine to keep the player "heated" for a specific duration (seconds)
    public IEnumerator FlameOn(float seconds)
    {
        // Check if _playerStats is assigned before proceeding
        if (_playerStats != null)
        {
            // Set the heated status to true for the duration
            _playerStats.heated = true;
            yield return new WaitForSeconds(seconds);
            _playerStats.heated = false; // Reset the heated status after the duration
        }
        else
        {
            // Log error if PlayerStats is not found
            Debug.LogError("PlayerStats not assigned!");
        }
    }
}
