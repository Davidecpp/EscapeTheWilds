using TMPro;  // For TextMeshPro text rendering
using UnityEngine;
using UnityEngine.UI;

public class ExpController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelTxt;      // Reference to the TextMeshProUGUI component for displaying the level
    [SerializeField] private TextMeshProUGUI _experienceTxt; // Reference to the TextMeshProUGUI component for displaying the experience
    [SerializeField] private Image ExpProgressBar;           // Reference to the Image component for displaying the experience progress bar

    private PlayerStats _playerStats;  // Reference to the PlayerStats script for getting the player's stats

    // Start is called before the first frame update
    private void Start()
    {
        // Reset the stored player experience and level in PlayerPrefs at the start of the game
        PlayerPrefs.DeleteKey("PlayerExp");
        PlayerPrefs.DeleteKey("PlayerLevel");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the PlayerStats reference is null (i.e., not assigned yet)
        if (_playerStats == null)
        {
            // Attempt to find the PlayerStats script in the scene
            _playerStats = FindObjectOfType<PlayerStats>();
            
            // If PlayerStats is found, set it as the reference
            if (_playerStats != null)
            {
                SetPlayerReference(_playerStats);
            }
            else
            {
                // If PlayerStats is not found, exit the method
                return;
            }
        }

        // Update the experience text (current experience / experience required for next level)
        _experienceTxt.text = _playerStats.GetExperience() + "/" + _playerStats.GetNextLevelExp();

        // Call the ExperienceController method to update level and progress bar
        ExperienceController();
    }

    // Set the PlayerStats reference when it's found
    private void SetPlayerReference(PlayerStats player)
    {
        _playerStats = player;
    }

    // Controls the experience bar and level text display
    private void ExperienceController()
    {
        // Update the level text with the player's current level
        _levelTxt.text = _playerStats.GetLevel().ToString();
        
        // Update the experience progress bar to reflect the player's experience progress towards the next level
        // The fill amount is calculated as the ratio of current experience to the next level's experience requirement
        ExpProgressBar.fillAmount = _playerStats.GetExperience() / _playerStats.GetNextLevelExp();
    }
}
