using UnityEngine;

public class ChooseSnake : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 
    [SerializeField] private bool _bonusObj;

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("SNAKE");
        return true;
    }
}