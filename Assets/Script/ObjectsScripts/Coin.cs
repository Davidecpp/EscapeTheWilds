using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear;
    
    // Audio
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;
    private bool isCollected = false; 

    public string InteractionPrompt => prompt;
    public bool bonusObj { get; private set; } = true;

    private void Start()
    {
        // Adding audio source dinamically
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pickupSound;
        audioSource.playOnAwake = false;
    }

    public bool Interact(Interactor interactor)
    {
        // if it's been collected return
        if (isCollected) return false;

        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.AddCoin(1);
            MissionManager missionManager = FindObjectOfType<MissionManager>();
            missionManager.AddProgress("Tutorial 2", 1);
            
            audioSource.Play();
            isCollected = true; 
            
            // Makes object invisible
            GetComponent<Renderer>().enabled = false;
            
            if (shouldDisappear)
            {
                // Wait for sound lenght to destroy 
                Destroy(gameObject, pickupSound.length);
            }
            return true;
        }
        else
        {
            Debug.LogError("Inventory not found in the scene.");
            return false;
        }
    }
}