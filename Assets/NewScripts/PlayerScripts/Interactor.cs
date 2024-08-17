using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;

    private readonly Collider[] _colliders = new Collider[3];
    private IInteractible _interactable;
    private IInteractible _previousInteractable;

    private void Update()
    {
        Interaction();
    }

    private void Interaction()
    {
        int numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, _colliders, interactableMask);

        if (numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractible>();

            if (_interactable != null)
            {
                if (_interactable != _previousInteractable)
                {
                    if (_previousInteractable != null)
                    {
                        HighlightObject(_previousInteractable, false);
                    }

                    HighlightObject(_interactable, true);
                    _previousInteractable = _interactable;
                }

                if (!_interactionPromptUI.isDisplayed && !_interactable.bonusObj)
                {
                    _interactionPromptUI.SetUp(_interactable.InteractionPrompt);
                }

                if (_interactable.bonusObj || Keyboard.current.eKey.wasPressedThisFrame)
                {
                    bool interacted = _interactable.Interact(this);
                    if (interacted)
                    {
                        _interactable = null;
                    }
                }
            }
        }
        else
        {
            if (_interactable != null)
            {
                if (_interactable is MonoBehaviour interactableMonoBehaviour && interactableMonoBehaviour != null)
                {
                    HighlightObject(_interactable, false);
                }
                _interactable = null;
                _previousInteractable = null;
            }
            if (_interactionPromptUI.isDisplayed) _interactionPromptUI.Close();
        }
    }

    private void HighlightObject(IInteractible interactible, bool highlight)
    {
        if (interactible is MonoBehaviour interactibleMonoBehaviour && interactibleMonoBehaviour != null)
        {
            var outline = interactibleMonoBehaviour.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = highlight;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
