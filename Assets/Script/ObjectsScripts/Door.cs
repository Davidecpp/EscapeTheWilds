using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private float openDistance = 5f;
    [SerializeField] private float animationDuration = 1f; 
    [SerializeField] private bool isDangerous; 
    private bool _isOpen = false; 
    private Vector3 _closedPosition; 
    private Vector3 _openPosition; 
    
    public string InteractionPrompt => prompt;

    public bool bonusObj => isDangerous; 

    private void Start()
    {
        _closedPosition = transform.localPosition;
        _openPosition = _closedPosition - new Vector3(0, openDistance, 0);
    }
    
    // Object interaction
    // Opening door with key
    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory == null) return false;
        if (isDangerous || inventory.hasKey)
        {
            Debug.Log("Opening door");
            StopAllCoroutines();
            StartCoroutine(MoveDoor());
            inventory.hasKey = false;
            return true;
        }
        Debug.Log("No key");
        return false;
    }

    private IEnumerator MoveDoor()
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = _isOpen ? _closedPosition : _openPosition;
        float timeElapsed = 0f;

        while (timeElapsed < animationDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = endPosition;
        _isOpen = !_isOpen; 
    }
}