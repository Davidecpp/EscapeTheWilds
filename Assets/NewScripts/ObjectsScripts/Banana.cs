using UnityEngine;

public class Banana : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 

    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true; 
    
    // Object interaction
    // Damage the player
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Banana picked");

        if (bonusObj)
        {
            GameManager.Instance.DecreaseHealth(); 
            Debug.Log("Danno preso");
        }

        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }
        return true;
    }
}