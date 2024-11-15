using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractible
{
    // The prompt displayed when the player can interact with the heart
    [SerializeField] private string prompt;
    
    // A flag that determines whether the heart should disappear after being picked up
    [SerializeField] private bool shouldDisappear;
    
    public float time = 1f; // Duration of the healing effect

    // Interaction prompt property
    public string InteractionPrompt => prompt;

    // Marks the heart as a bonus object
    public bool bonusObj { get; private set; } = true;
    
    // Reference to the player's stats (health)
    private PlayerStats _playerStats;

    // Initialize the PlayerStats reference
    private void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    // Interaction with the heart: increases health
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Heart picked");
        // Add a heart to the player's health
        _playerStats.AddHeart();
        
        // Start healing effect
        StartCoroutine(HealForSeconds(time));

        // If the heart should disappear, disable the renderer and collider, then destroy it after the healing time
        if (shouldDisappear)
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, time + 0.1f); 
        }

        return true;
    }

    // Coroutine for healing over a set duration
    private IEnumerator HealForSeconds(float seconds)
    {
        GameManager.Instance.healing = true; // Set healing true to activate particles
        
        // Wait for the specified amount of time and deactivate particles
        yield return new WaitForSeconds(seconds); 
        GameManager.Instance.healing = false;
    }
}