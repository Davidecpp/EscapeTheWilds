using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Interface for interactable objects
public interface IInteractible
{
    public string InteractionPrompt { get; } // txt displayed

    public bool Interact(Interactor interactor);
    bool bonusObj { get; } 
}
