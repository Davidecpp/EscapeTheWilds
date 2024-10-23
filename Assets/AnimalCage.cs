using System;
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

    private MenuManager _menu;

    private void Start()
    {
        _menu = FindObjectOfType<MenuManager>();
    }

    // Object interaction
    // Choose animal
    public bool Interact(Interactor interactor)
    {
        if (animalName.Equals("deer"))
        {
            SelectCharacter(0);
        }
        if (animalName.Equals("hamster"))
        {
            SelectCharacter(1);
        }
        if (animalName.Equals("monkey"))
        {
            SelectCharacter(2);
        }
        if (animalName.Equals("snake"))
        {
            SelectCharacter(3);
        }
        Debug.Log("Cage");
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
        SceneManager.LoadScene(5);
    }
}