using UnityEngine;

public class Bullet : MonoBehaviour, IInteractible
{
    // The prompt displayed when the player can interact with the bullet
    [SerializeField] private string prompt;
    
    // A flag that determines whether the bullet should disappear after being picked up
    [SerializeField] private bool shouldDisappear;
    
    // Interaction prompt property
    public string InteractionPrompt => prompt;

    // Marks the bullet as a bonus object
    public bool bonusObj { get; private set; } = true;
    
    // Interaction with the bullet: adds 1 ammo to the inventory
    public bool Interact(Interactor interactor)
    {
        // Find the inventory component in the scene
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            // Add 1 bullet to the inventory
            inventory.AddBullet(1);

            // If the bullet should disappear, destroy the object
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