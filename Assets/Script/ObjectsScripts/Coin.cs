using UnityEngine;

public class Coin : MonoBehaviour, IInteractible
{
    // The prompt displayed when the player can interact with the coin
    [SerializeField] private string prompt;
    
    // A flag that determines whether the coin should disappear after being picked up
    [SerializeField] private bool shouldDisappear;
    
    // Audio for the coin pickup sound
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;
    
    // Flag to check if the coin has been collected
    private bool isCollected = false; 

    // Interaction prompt property
    public string InteractionPrompt => prompt;

    // Marks the coin as a bonus object
    public bool bonusObj { get; private set; } = true;

    // Initialize the audio source dynamically
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pickupSound;
        audioSource.playOnAwake = false;
    }

    // Interaction with the coin: adds 1 coin to the inventory
    public bool Interact(Interactor interactor)
    {
        // If the coin has already been collected, return false
        if (isCollected) return false;

        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            // Add 1 coin to the inventory
            inventory.AddCoin(1);
            
            // Update mission progress
            MissionManager missionManager = FindObjectOfType<MissionManager>();
            missionManager.AddProgress("Tutorial 2", 1);

            // Play the pickup sound
            audioSource.Play();
            
            // Mark the coin as collected
            isCollected = true; 
            
            // Make the coin invisible
            GetComponent<Renderer>().enabled = false;
            
            // If the coin should disappear, destroy it after the sound has finished playing
            if (shouldDisappear)
            {
                Destroy(gameObject, pickupSound.length);
            }

            return true;
        }
        else
        {
            // Log an error if the inventory component is not found
            Debug.LogError("Inventory not found in the scene.");
            return false;
        }
    }
}
