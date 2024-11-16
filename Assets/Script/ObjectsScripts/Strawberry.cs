using UnityEngine;
using UnityEngine.SceneManagement;

public class Strawberry : MonoBehaviour, IInteractible
{
    // Serialized fields allow customization in the Unity Inspector
    [SerializeField] private string prompt;          // Interaction prompt displayed to the player
    [SerializeField] private bool shouldDisappear;   // Determines if the strawberry should disappear after being collected
    [SerializeField] private bool _bonusObj;         // Indicates if this strawberry is a bonus object

    // Public properties for accessing interaction details
    public string InteractionPrompt => prompt;       // Returns the interaction prompt
    public bool bonusObj => _bonusObj;               // Returns whether this is a bonus object

    // Handles interaction with the strawberry object
    // Called when the player interacts with the strawberry
    public bool Interact(Interactor interactor)
    {
        // Find the Inventory and PlayerStats components in the scene
        Inventory inventory = FindObjectOfType<Inventory>();
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        // Ensure both Inventory and PlayerStats exist before proceeding
        if (inventory != null && playerStats != null)
        {
            // Add one strawberry to the inventory
            inventory.AddStrawberry(1);

            // Grant the player 50 experience points
            playerStats.AddExperience(50);
            
            // Update mission progress in a certain scene
            if (SceneManager.GetActiveScene().name.Equals("lvl.5"))
            {
                FindObjectOfType<MissionManager>().AddProgress("Desert", 1);
            }

            // If the strawberry is set to disappear, destroy it from the scene
            if (shouldDisappear)
            {
                Destroy(gameObject);
            }

            // Interaction was successful
            return true;
        }
        else
        {
            // Log an error if the necessary components are missing
            Debug.LogError("Inventory or PlayerStats not found in the scene.");
            return false;
        }
    }
}