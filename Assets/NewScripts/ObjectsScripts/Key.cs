using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 
    [SerializeField] private bool _bonusObj;

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;
    
    // Object interaction
    // Pick key
    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();
        inventory.hasKey = true;
        Debug.Log("Key picked");
        
        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }
        return true;
    }
}
