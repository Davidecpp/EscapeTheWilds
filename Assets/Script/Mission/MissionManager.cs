using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public List<Mission> missions;
    private MissionUI missionUI;
    public int activeMissionIndex = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        missionUI = FindObjectOfType<MissionUI>();
    }

    private void Update()
    {
        activeMissionIndex = SceneManager.GetActiveScene().buildIndex - 5;
        
        if (activeMissionIndex >= 0 && activeMissionIndex < missions.Count)
        {
            missionUI.missionPanel.SetActive(true);
            missionUI.UpdateUI();
        }
        else
        {
            missionUI.missionPanel.SetActive(false);
            Debug.LogWarning("Active mission index is out of range.");
        }

        Debug.Log("Next scene: " + missionUI._nextScene);
    }

    // Add progress to a determined mission
    public void AddProgress(string missionTitle, int amount)
    {
        Mission mission = missions.Find(m => m.title == missionTitle);
        
        if (mission != null && !mission.isCompleted)
        {
            mission.currentAmount += amount;
            mission.CheckCompletion();
            missionUI.UpdateUI();
        }
        else
        {
            Debug.LogWarning($"Mission '{missionTitle}' not found or already completed.");
        }
    }
    
    // Reset all mission statuses
    public void ResetMissionStatus()
    {
        foreach (Mission mission in missions)
        {
            mission.currentAmount = 0;
            mission.isCompleted = false;
        }
    }
    
    // Reset the amount of a specific mission by index
    public void ResetMissionAmount(int index)
    {
        if (index >= 0 && index < missions.Count)
        {
            missions[index].currentAmount = 0;
        }
        else
        {
            Debug.LogWarning("ResetMissionAmount index out of range.");
        }
    }
}