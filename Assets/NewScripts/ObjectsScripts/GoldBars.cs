using System.Collections;
using UnityEngine;

public class GoldBars : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;
    
    // Object interaction
    // Gives +10 coin
    public bool Interact(Interactor interactor)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.AddCoin(10);
            Debug.Log("GoldBars picked");

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