using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    // The prompt shown when the player is close to interact with the door
    [SerializeField] private string prompt;

    // Distance the door will move when opened
    [SerializeField] private float openDistance = 5f;

    // Duration of the door opening animation
    [SerializeField] private float animationDuration = 1f; 

    // Flag to determine if the door is dangerous (could trigger a hazard)
    [SerializeField] private bool isDangerous; 

    // Flag to track whether the door is open or closed
    private bool _isOpen = false; 

    // Store the door's position when closed and when open
    private Vector3 _closedPosition; 
    private Vector3 _openPosition; 
    
    // The interaction prompt shown to the player
    public string InteractionPrompt => prompt;

    // If the door is dangerous, it is considered a bonus object
    public bool bonusObj => isDangerous; 

    private void Start()
    {
        // Initialize the closed and open positions based on the current local position of the door
        _closedPosition = transform.localPosition;
        _openPosition = _closedPosition - new Vector3(0, openDistance, 0); // Move door downward by openDistance
    }
    
    // Interaction with the door (opening it)
    // Requires a key or allows if the door is dangerous
    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        // If no inventory or player doesn't have a key, return false
        if (inventory == null) return false;

        // If the door is dangerous or the player has the key, start opening the door
        if (isDangerous || inventory.hasKey)
        {
            StopAllCoroutines(); // Stop any existing door opening/closing animations
            StartCoroutine(MoveDoor()); // Start the door opening animation
            inventory.hasKey = false; // Use up the key after interacting with the door
            return true;
        }
        
        // Return false if conditions are not met
        return false;
    }

    // Coroutine to move the door from the closed to open position (or vice versa)
    private IEnumerator MoveDoor()
    {
        // Store the initial position of the door (current state)
        Vector3 startPosition = transform.localPosition;

        // Determine the target position (open or closed) based on the current state of the door
        Vector3 endPosition = _isOpen ? _closedPosition : _openPosition;

        // Track the time elapsed during the animation
        float timeElapsed = 0f;

        // Animate the door opening/closing using a smooth transition (Lerp)
        while (timeElapsed < animationDuration)
        {
            // Lerp between start and end positions based on the elapsed time
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the door reaches the exact end position at the end of the animation
        transform.localPosition = endPosition;

        // Toggle the state of the door (open/closed)
        _isOpen = !_isOpen; 
    }
}
