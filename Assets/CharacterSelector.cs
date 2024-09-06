using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int selectedCharacterID = PlayerPrefs.GetInt("SelectedCharacter");
        Debug.Log("ID del personaggio selezionato: " + selectedCharacterID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
