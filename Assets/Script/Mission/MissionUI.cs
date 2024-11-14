using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionUI : MonoBehaviour
{
    public GameObject missionPanel;
    public TextMeshProUGUI missionText;
    public TextMeshProUGUI missionRewardText;
    public TextMeshProUGUI expRewardText;
    public GameObject rewardPanel;
    private MissionManager missionManager;
    public int _nextScene = 6;

    public AudioSource audioSource;

    private void Start()
    {
        missionManager = FindObjectOfType<MissionManager>();
        UpdateUI();
    }
    
    // Update UI mission progress
    public void UpdateUI()
    {
        missionText.text = "";
        
        if (missionManager.activeMissionIndex >= 0 && missionManager.activeMissionIndex < missionManager.missions.Count)
        {
            Mission mission = missionManager.missions[missionManager.activeMissionIndex];
            missionText.text += $"{mission.title}:\n{mission.description}\n {mission.currentAmount}/{mission.goalAmount}\n";
        }
        else
        {
            Debug.LogWarning("Active mission index is out of range.");
        }
    }
    
    // Show reward panel
    public void RewardUI()
    {
        GameManager.Instance.PauseGame();
        rewardPanel.SetActive(true);

        audioSource.Play();

        missionRewardText.text = "";
        expRewardText.text = "";
        
        if (missionManager.activeMissionIndex >= 0 && missionManager.activeMissionIndex < missionManager.missions.Count)
        {
            Mission mission = missionManager.missions[missionManager.activeMissionIndex];
            missionRewardText.text += $"x{mission.reward}";
            expRewardText.text += $"Exp + {mission.expReward}";
        }
    }

    public void AcceptReward()
    {
        rewardPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene(_nextScene);

        // Display specific dialogues based on the next scene
        if (_nextScene == 6)
        {
            FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Collect all the coins.", "Press SPACE to jump." });
        }
        if (_nextScene == 7)
        {
            FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Kill all the enemies.", "Press LEFT-MOUSE to attack." });
        }
        _nextScene++;
    }
}
