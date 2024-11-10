using System;
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
    private int _nextScene = 6;
    
    public AudioSource audioSource;

    private void Start()
    {
        missionManager = FindObjectOfType<MissionManager>();
        UpdateUI();
        missionPanel.SetActive(false);
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
        expRewardText.text = "";
        Mission mission = missionManager.missions[missionManager.activeMissionIndex];
        missionRewardText.text += $"x{mission.reward}";
        expRewardText.text += $"Exp + {mission.expReward}";
    }

    public void AcceptReward()
    {
        rewardPanel.SetActive(false);
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene(_nextScene);
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