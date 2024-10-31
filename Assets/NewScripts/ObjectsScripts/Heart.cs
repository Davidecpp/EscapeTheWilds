using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;
    
    // Object interaction
    // Gives +1 health
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Heart picked");
        PlayerStats.Instance.AddHeart();
        GameManager.Instance.healing = true;

        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }
        return true;
    }
}