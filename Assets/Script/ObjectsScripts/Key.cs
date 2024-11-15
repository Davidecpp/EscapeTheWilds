using UnityEngine;

public class Key : MonoBehaviour, IInteractible
{
    // The prompt that will be shown to the player when they can interact with the key
    [SerializeField] private string prompt;
    
    // A flag indicating whether the key should disappear after being picked up
    [SerializeField] private bool shouldDisappear; 
    
    // A flag indicating whether the key is a bonus object
    [SerializeField] private bool _bonusObj;

    // Property to retrieve the interaction prompt
    public string InteractionPrompt => prompt;
    
    // Property to check if this is a bonus object
    public bool bonusObj => _bonusObj;
    
    // Method that handles the interaction with the key
    // When the player interacts with the key, it will be added to their inventory
    public bool Interact(Interactor interactor)
    {
        // Get the player's inventory component
        var inventory = interactor.GetComponent<Inventory>();
        
        // Mark the key as acquired by setting hasKey to true in the player's inventory
        inventory.hasKey = true;
        
        // Update mission progress, for example, completing a tutorial task
        FindObjectOfType<MissionManager>().AddProgress("Tutorial", 1);
        
        // If the flag shouldDisappear is true, destroy the key object (make it disappear from the scene)
        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }
        
        // Return true indicating the interaction was successful
        return true;
    }
}