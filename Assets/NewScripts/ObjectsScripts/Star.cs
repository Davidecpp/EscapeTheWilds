using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    public float time;

    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Star picked");
        StartCoroutine(MakeInvincibleForSeconds(time));
        if (shouldDisappear)
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, time + 0.1f);
        }

        return true;
    }

    private IEnumerator MakeInvincibleForSeconds(float seconds)
    {
        GameManager.Instance.invincible = true;
        Debug.Log("Invincibility started");
        yield return new WaitForSeconds(seconds); 
        GameManager.Instance.invincible = false;
        Debug.Log("Invincibility ended");
    }

}