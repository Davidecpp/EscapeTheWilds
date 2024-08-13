using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;

    public string InteractionPrompt => prompt;
    
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Chest opned");
        return true;
    }

    public bool bonusObj { get; }
}
