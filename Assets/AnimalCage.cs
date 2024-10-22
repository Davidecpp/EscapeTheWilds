using System;
using UnityEngine;

public class AnimalCage : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 
    [SerializeField] private bool _bonusObj;

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;
    public String animalName;
    
    // Object interaction
    // Choose animal
    public bool Interact(Interactor interactor)
    {
        if (animalName.Equals("monkey"))
        {
            
        }
        Debug.Log("Cage");
        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }
        return true;
    }
}