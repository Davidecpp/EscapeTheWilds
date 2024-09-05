using System.Collections;
using UnityEngine;

public class Pepper : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    

    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Pepper picked");
        
        StartCoroutine(FlameOn(5.0f));
        if (shouldDisappear)
        {
            Destroy(gameObject);
        }
        return true;
    }

    public IEnumerator FlameOn(float seconds)
    {
        Debug.Log("Heated");
        GameManager.Instance.heated = true;
        yield return new WaitForSeconds(seconds);
        GameManager.Instance.heated = false;
    }
    

}