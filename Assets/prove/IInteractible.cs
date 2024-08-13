using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    public string InteractionPrompt { get; }

    public bool Interact(Interactor interactor);
    bool bonusObj { get; } 
}
