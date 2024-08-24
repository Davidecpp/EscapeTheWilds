using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    //public GameObject particlesPrefab; 
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Heart picked");
        GameManager.Instance.IncreaseHealth();
        GameManager.Instance.healing = true;
        //StartCoroutine(MakeHealForSeconds(1f));
        //GenerateParticles(interactor.transform.position);

        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }

        return true;
    }

    /*private IEnumerator MakeHealForSeconds(float seconds)
    {
        GameManager.Instance.healing = true;
        Debug.Log("Healing started");
        yield return new WaitForSeconds(seconds); 
        GameManager.Instance.healing = false;
        Debug.Log("Healing ended");
    }*/
}