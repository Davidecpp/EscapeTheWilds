using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<Mission> missions;
    private MissionUI missionUI;
    public int activeMissionIndex = 0;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        missionUI = FindObjectOfType<MissionUI>();
        if (activeMissionIndex == 1)
        {
            FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Explore the maze and find the key." });
        }
    }

    private void Update()
    {
        
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