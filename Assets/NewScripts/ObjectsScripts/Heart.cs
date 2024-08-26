using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Heart picked");
        GameManager.Instance.IncreaseHealth();
        GameManager.Instance.healing = true;

        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }

        return true;
    }
}