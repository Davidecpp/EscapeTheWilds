using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject horse;
    public GameObject tiger;
    public GameObject deer;
    void Awake()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter");
        if(selectedCharacter == "Horse")
        {
            horse.SetActive(true);
            tiger.SetActive(false);
            deer.SetActive(false);
        }
        else if(selectedCharacter == "Tiger")
        {
            horse.SetActive(false);
            tiger.SetActive(true);
            deer.SetActive(false);
        }
        else if(selectedCharacter == "Deer")
        {
            horse.SetActive(false);
            tiger.SetActive(false);
            deer.SetActive(true);
        }
        
    }
}