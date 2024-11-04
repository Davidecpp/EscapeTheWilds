using UnityEngine;
using TMPro;

public class MissionUI : MonoBehaviour
{
    public TextMeshProUGUI missionText;
    public TextMeshProUGUI missionRewardText;
    public GameObject rewardPanel;
    private MissionManager missionManager;
    
    public AudioSource audioSource;

    private void Start()
    {
        missionManager = FindObjectOfType<MissionManager>();
        UpdateUI();
    }

    public void UpdateUIForEachMission()
    {
        missionText.text = "";
        foreach (Mission mission in missionManager.missions)
        {
            missionText.text += $"{mission.title}:\n{mission.description}\n {mission.currentAmount}/{mission.goalAmount}\n";
        }
    }
    // Update UI mission progress
    public void UpdateUI()
    {
        missionText.text = "";
        Mission mission = missionManager.missions[missionManager.activeMissionIndex];
        missionText.text += $"{mission.title}:\n{mission.description}\n {mission.currentAmount}/{mission.goalAmount}\n";
    }
    
    // Show reward panel
    public void RewardUI()
    {
        GameManager.Instance.PauseGame();
        rewardPanel.SetActive(true);
        
        audioSource.Play();
        
        missionRewardText.text = "";
        Mission mission = missionManager.missions[missionManager.activeMissionIndex];
        missionRewardText.text += $"x{mission.reward}"; 
    }

    public void AcceptReward()
    {
        rewardPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}