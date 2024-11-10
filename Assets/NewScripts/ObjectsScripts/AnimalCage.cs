using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimalCage : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 
    [SerializeField] private bool _bonusObj;

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;
    public String animalName;

    // Object interaction
    // Choose animal
    public bool Interact(Interactor interactor)
    {
        FindObjectOfType<Dialogue>().SetDialogue(new string[] { "You choose " + animalName, "Let's see how he behaves..." });
        StartCoroutine(WaitForDialogue());
        
        if (shouldDisappear)
        {
            Destroy(gameObject); 
        }
        return true;
    }
    public void SelectCharacter(int characterID)
    {
        // Saves selected character's ID
        PlayerPrefs.SetInt("SelectedCharacter", characterID);
        Debug.Log("Selected: " + characterID);

        // Change scene after choosing a character
        GameManager.Instance.currentScene++;
        SceneManager.LoadScene(5);
        FindObjectOfType<MissionUI>().missionPanel.SetActive(true);
        FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Explore the maze and find the key." });
    }
    private void SelectCharacterAfterDialogue()
    {
        switch (animalName)
        {
            case "deer":
                SelectCharacter(0);
                break;
            case "hamster":
                SelectCharacter(1);
                break;
            case "monkey":
                SelectCharacter(2);
                break;
            case "snake":
                SelectCharacter(3);
                break;
        }
    }

    private IEnumerator WaitForDialogue()
    {
        Dialogue dialogue = FindObjectOfType<Dialogue>();

        // Wait for dialogue to end
        while (dialogue.isActive)
        {
            yield return null;
        }
        SelectCharacterAfterDialogue();
    }
}