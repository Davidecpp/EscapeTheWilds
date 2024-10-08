using System.Collections;
using UnityEngine;

public class Apple : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;
    
    // Object interaction
    // Boost player speed for a limited period of time
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Apple picked");

        Movement playerMovement = interactor.GetComponent<Movement>();
        if (playerMovement != null)
        {
            playerMovement.BoostSpeed(5.0f); 
        }

        if (shouldDisappear)
        {
            Destroy(gameObject);
        }
        return true;
    }
}