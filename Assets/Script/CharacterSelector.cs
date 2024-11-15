using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // Array of character prefabs that can be selected
    public GameObject[] characterPrefabs;
    
    // Private variable to hold the selected character instance
    private GameObject _selectedCharacter;
    
    // The spawn point where the selected character will appear
    public GameObject playerSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the selected character ID from PlayerPrefs (default to -1 if not set)
        int selectedCharacterID = PlayerPrefs.GetInt("SelectedCharacter", -1);
        
        // Check if the retrieved ID is valid (within range of available characters)
        if (selectedCharacterID >= 0 && selectedCharacterID < characterPrefabs.Length)
        {
            // Instantiate the selected character at the player spawn position and rotation
            _selectedCharacter = Instantiate(characterPrefabs[selectedCharacterID], playerSpawn.transform.position, playerSpawn.transform.rotation);
        }
        else
        {
            // Log an error if the character ID is invalid or not found
            Debug.LogError("Selected character ID is invalid or not present!");
        }
    }
}