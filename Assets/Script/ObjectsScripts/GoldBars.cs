using UnityEngine;

public class GoldBars : MonoBehaviour, IInteractible
{
    // The prompt displayed when the player can interact with the gold bars
    [SerializeField] private string prompt;
    
    // A flag that determines whether the gold bars should disappear after being picked up
    [SerializeField] private bool shouldDisappear;
    
    // Audio for the coin pickup sound
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;
    
    // Interaction prompt property
    public string InteractionPrompt => prompt;

    // Marks the gold bars as a bonus object
    public bool bonusObj { get; private set; } = true;
    
    // Initialize the audio source dynamically
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pickupSound;
        audioSource.playOnAwake = false;
    }
    
    // Interaction with the gold bars: adds 10 coins to the inventory
    public bool Interact(Interactor interactor)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            // Add 10 coins to the inventory
            inventory.AddCoin(10);
            
            // Play the pickup sound
            audioSource.Play();
            
            // If the gold bars should disappear, destroy the object
            if (shouldDisappear)
            {
                Destroy(gameObject); 
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