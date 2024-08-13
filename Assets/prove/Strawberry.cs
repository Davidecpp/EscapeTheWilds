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
        if (inventory != null)
        {
            inventory.AddStrawberry();
            Debug.Log("Strawberry picked");

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