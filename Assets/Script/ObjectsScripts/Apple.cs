using System.Collections;
using UnityEngine;

public class Apple : MonoBehaviour, IInteractible
{
    // The prompt displayed when the player is close enough to interact with the apple
    [SerializeField] private string prompt;

    // Flag to determine if the apple should disappear after being interacted with
    [SerializeField] private bool shouldDisappear;

    // Property to return the interaction prompt
    public string InteractionPrompt => prompt;

    // Bonus object flag to indicate if the apple provides a bonus to the player
    public bool bonusObj { get; private set; } = true;
    
    // Object interaction with the player
    // Boosts the player's speed for a short period of time
    public bool Interact(Interactor interactor)
    {
        // Find the CanvasManager to access the BoostSpeed method
        var canvas = FindObjectOfType<CanvasManager>();

        // Check if CanvasManager is found in the scene
        if (canvas != null)
        {
            // Boost player's speed for 5 seconds
            canvas.BoostSpeed(5.0f);
        }

        // If the apple should disappear, destroy the apple object after interaction
        if (shouldDisappear)
        {
            Destroy(gameObject);
        }

        // Return true to indicate that the interaction was successful
        return true;
    }
}