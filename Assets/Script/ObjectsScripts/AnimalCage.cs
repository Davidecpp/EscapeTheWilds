using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimalCage : MonoBehaviour, IInteractible
{
    // Prompt shown when the player can interact with the animal cage
    [SerializeField] private string prompt;
    
    // Flag indicating whether the object should disappear after interaction
    [SerializeField] private bool shouldDisappear; 
    
    // Flag to mark if the object is a bonus item
    [SerializeField] private bool _bonusObj;

    // Interaction prompt property
    public string InteractionPrompt => prompt;

    // Bonus object flag property
    public bool bonusObj => _bonusObj;

    // Name of the animal associated with the cage
    public String animalName;

    // Interaction with the animal cage: sets dialogue and starts a coroutine to wait for dialogue completion
    public bool Interact(Interactor interactor)
    {
        // Set dialogue to notify the player about choosing the animal
        FindObjectOfType<Dialogue>().SetDialogue(new string[] { "You choose " + animalName, "Let's see how he behaves..." });
        
        // Start the coroutine to wait for dialogue completion before selecting a character
        StartCoroutine(WaitForDialogue());
        
        // If the object should disappear after interaction, destroy it
        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }
        
        return true;
    }

    // Selects a character based on the passed character ID and changes scene
    public void SelectCharacter(int characterID)
    {
        // Save the selected character's ID in PlayerPrefs
        PlayerPrefs.SetInt("SelectedCharacter", characterID);

        // Increment the current scene and load the next scene
        GameManager.Instance.currentScene++;
        SceneManager.LoadScene("lvl.1");
        
        // Activate mission panel and set dialogue for the new scene
        FindObjectOfType<MissionUI>().missionPanel.SetActive(true);
        FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Explore the maze and find the key.","Press SHIFT to run." });
    }

    // Select the character based on the animal name
    private void SelectCharacterAfterDialogue()
    {
        // Switch case to select a character based on the animal name
        switch (animalName)
        {
            case "deer":
                SelectCharacter(0); // Select deer
                break;
            case "hamster":
                SelectCharacter(1); // Select hamster
                break;
            case "monkey":
                SelectCharacter(2); // Select monkey
                break;
            case "snake":
                SelectCharacter(3); // Select snake
                break;
        }
    }

    // Coroutine to wait until the dialogue is no longer active before selecting the character
    private IEnumerator WaitForDialogue()
    {
        Dialogue dialogue = FindObjectOfType<Dialogue>();

        // Wait for the dialogue to end
        while (dialogue.isActive)
        {
            yield return null;
        }

        // After dialogue ends, select the character based on the animal name
        SelectCharacterAfterDialogue();
    }
}
