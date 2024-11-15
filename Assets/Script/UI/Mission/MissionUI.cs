using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionUI : MonoBehaviour
{
    // UI Elements for displaying mission info and rewards
    public GameObject missionPanel;                  // The panel that shows mission details
    public TextMeshProUGUI missionText;               // Text displaying mission title and progress
    public TextMeshProUGUI missionRewardText;         // Text displaying reward (coins)
    public TextMeshProUGUI expRewardText;             // Text displaying reward (experience points)
    public GameObject rewardPanel;                   // The panel showing the reward UI
    private MissionManager missionManager;            // Reference to the MissionManager for mission details
    public int _nextScene = 6;                        // The next scene to load after accepting the reward

    public AudioSource audioSource;                   // AudioSource for reward sound effect

    // Start is called before the first frame update
    private void Start()
    {
        // Cache references to avoid repeated calls to FindObjectOfType
        missionManager = FindObjectOfType<MissionManager>();  // Get the MissionManager in the scene
        UpdateUI();                                           // Update the UI to reflect the current mission progress
    }

    // Update the UI to display the current mission's title, description, and progress
    public void UpdateUI()
    {
        missionText.text = "";  // Clear any previous text

        // Ensure the active mission index is within the range of available missions
        if (missionManager.activeMissionIndex >= 0 && missionManager.activeMissionIndex < missionManager.missions.Count)
        {
            // Get the current mission and display its details
            Mission mission = missionManager.missions[missionManager.activeMissionIndex];
            missionText.text += $"{mission.title}:\n{mission.description}\n {mission.currentAmount}/{mission.goalAmount}\n";
        }
        else
        {
            // Log a warning if the active mission index is out of range
            Debug.LogWarning("Active mission index is out of range.");
        }
    }

    // Display the reward panel when the mission is completed
    public void RewardUI()
    {
        // Pause the game to focus on the reward UI
        GameManager.Instance.PauseGame();
        rewardPanel.SetActive(true);  // Show the reward panel

        // Play the reward sound effect
        audioSource.Play();

        // Clear any previous reward text
        missionRewardText.text = "";
        expRewardText.text = "";

        // Ensure the active mission index is within range before accessing mission details
        if (missionManager.activeMissionIndex >= 0 && missionManager.activeMissionIndex < missionManager.missions.Count)
        {
            // Get the current mission and display its rewards
            Mission mission = missionManager.missions[missionManager.activeMissionIndex];
            missionRewardText.text += $"x{mission.reward}";         // Display coin reward
            expRewardText.text += $"Exp + {mission.expReward}";      // Display experience reward
        }
    }

    // Handles accepting the reward and transitioning to the next scene
    public void AcceptReward()
    {
        rewardPanel.SetActive(false);  // Hide the reward panel
        GameManager.Instance.ResumeGame();  // Resume the game after accepting the reward

        // Load the next scene based on the current scene index
        SceneManager.LoadScene(_nextScene);

        // Display specific dialogues based on the next scene
        SetNextSceneDialogue();  // Update dialogue based on the scene
        _nextScene++;            // Increment to the next scene for future use
    }

    // Sets the dialogues specific to the next scene
    private void SetNextSceneDialogue()
    {
        // Check the next scene number and set the corresponding dialogue
        switch (_nextScene)
        {
            case 6:
                // If the next scene is 6, set the dialogue to instructions for coin collection
                FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Collect all the coins.", "Press SPACE to jump." });
                break;
            case 7:
                // If the next scene is 7, set the dialogue for killing enemies
                FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Kill all the enemies.", "Press LEFT-MOUSE to attack." });
                break;
            // Add more cases here for other scenes as needed
        }
    }
}
