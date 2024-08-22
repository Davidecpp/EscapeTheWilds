using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;

    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Star picked");
        StartCoroutine(MakeInvincibleForSeconds(3f));
        if (shouldDisappear)
        {
            Destroy(gameObject);
        }

        return true;
    }

    private IEnumerator MakeInvincibleForSeconds(float seconds)
    {
        GameManager.Instance.invincible = true;
        yield return new WaitForSeconds(seconds); 
        GameManager.Instance.invincible = false;
    }
}