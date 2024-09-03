using UnityEngine;

public class Strawberry : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 
    [SerializeField] private bool _bonusObj;

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;

    public bool Interact(Interactor interactor)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        
        if (inventory != null && playerStats != null)
        {
            inventory.AddStrawberry();
            playerStats.AddExperience(50);

            if (shouldDisappear)
            {
                Destroy(gameObject); 
            }
            return true;
        }
        else
        {
            Debug.LogError("Inventory not found in the scene.");
            return false;
        }
    }
}