using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private GameObject selectedCharacter;
    public GameObject playerSpawn;

    void Start()
    {
        int selectedCharacterID = PlayerPrefs.GetInt("SelectedCharacter", -1);
        Debug.Log("ID del personaggio selezionato: " + selectedCharacterID);
        Debug.Log("Posizione di playerSpawn: " + playerSpawn.transform.position);
        
        if (selectedCharacterID >= 0 && selectedCharacterID < characterPrefabs.Length)
        {
            selectedCharacter = Instantiate(characterPrefabs[selectedCharacterID], playerSpawn.transform.position, playerSpawn.transform.rotation);
            Debug.Log("Personaggio selezionato istanziato: " + selectedCharacter.name);
        }
        else
        {
            Debug.LogError("ID del personaggio selezionato non valido o non presente!");
        }
    }
}

