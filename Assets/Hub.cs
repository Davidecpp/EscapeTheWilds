using UnityEngine;

public class Hub : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 
    [SerializeField] private bool _bonusObj;

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interazione");
        return true;
    }
}