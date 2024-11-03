using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<Mission> missions;
    private MissionUI missionUI;

    private void Start()
    {
        missionUI = FindObjectOfType<MissionUI>();
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
    }

    public void DisplayMissionStatus()
    {
        foreach (Mission mission in missions)
        {
            Debug.Log($"{mission.title}: {mission.currentAmount}/{mission.goalAmount} - Completed: {mission.isCompleted}");
        }
    }
}