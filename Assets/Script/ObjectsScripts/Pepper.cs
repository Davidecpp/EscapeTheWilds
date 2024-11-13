using System;
using System.Collections;
using UnityEngine;

public class Pepper : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    
    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    // Object interaction
    // Makes bullets inflammable for a short period of time
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
        _playerStats.heated = true;
        yield return new WaitForSeconds(seconds);
        _playerStats.heated = false;
    }
    

}