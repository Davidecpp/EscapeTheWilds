using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;  // The point from which interaction is detected (e.g., in front of the player)
    [SerializeField] private float interactionPointRadius;  // The radius within which interactable objects are detected
    [SerializeField] private LayerMask interactableMask;  // The layer mask to specify which objects are interactable
    [SerializeField] private InteractionPromptUI _interactionPromptUI;  // UI element that shows the interaction prompt

    private readonly Collider[] _colliders = new Collider[3];  // Array of colliders to hold objects found within the interaction radius
    private IInteractible _interactable;  // Currently interactable object
    private IInteractible _previousInteractable;  // Previously interactable object (used for comparison)

    // Update is called once per frame
    private void Update()
    {
        Interaction();  // Handle interaction logic each frame
    }
    
    // Handles the interaction logic with objects
    private void Interaction()
    {
        // Check for all objects within the interaction radius using OverlapSphere
        // The result is stored in _colliders, and the number of found objects is stored in numFound
        int numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, _colliders, interactableMask);

        // If at least one object is found in the interaction radius
        if (numFound > 0)
        {
            // Try to get the IInteractible component from the first collider
            _interactable = _colliders[0].GetComponent<IInteractible>();

            // If the object is interactable, handle the interaction
            if (_interactable != null)
            {
                // If interaction prompt is not already displayed and it's not a bonus object
                if (!_interactionPromptUI.isDisplayed && !_interactable.bonusObj)
                {
                    // Show the interaction prompt UI with the text defined in InteractionPrompt
                    _interactionPromptUI.SetUp(_interactable.InteractionPrompt);
                }

                // If the object is a bonus object or the 'E' key is pressed, trigger interaction
                if (_interactable.bonusObj || Keyboard.current.eKey.wasPressedThisFrame)
                {
                    bool interacted = _interactable.Interact(this);  // Perform the interaction with the object
                    if (interacted)
                    {
                        _interactable = null;  // Reset the interactable object after interaction
                    }
                }
            }
        }
        else
        {
            // If no objects are found in the interaction radius, clear the current interactable
            if (_interactable != null)
            {
                _interactable = null;
                _previousInteractable = null;
            }
            
            // If the interaction prompt is still displayed, close it
            if (_interactionPromptUI.isDisplayed) _interactionPromptUI.Close();
        }
    }
    
    // Gizmos for visualizing the interaction radius in the editor
    private void OnDrawGizmos()
    {
        // Draw a red wireframe sphere at the interaction point to represent the interaction radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
