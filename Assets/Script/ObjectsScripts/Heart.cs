using System;
using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    public float time = 1f;
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;
    
    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    // Object interaction
    // Gives +1 health
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Heart picked");
        _playerStats.AddHeart();
        StartCoroutine(HealForSeconds(time));

        if (shouldDisappear)
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject,time + 0.1f); 
        }
        return true;
    }
    private IEnumerator HealForSeconds(float seconds)
    {
        GameManager.Instance.healing = true;
        Debug.Log("Healing started");
        yield return new WaitForSeconds(seconds); 
        GameManager.Instance.healing = false;
        Debug.Log("Healing ended");
    }
}