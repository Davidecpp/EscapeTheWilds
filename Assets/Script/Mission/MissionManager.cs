using System;
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
        if (activeMissionIndex >= 0)
        {
            missionUI.missionPanel.SetActive(true);
            missionUI.UpdateUI();
        }
        else
        {
            missionUI.missionPanel.SetActive(false);
        }
        Debug.Log("nextscene "+missionUI._nextScene);
        
        
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
    
    // Display the mission status for each mission
    public void ResetMissionStatus()
    {
        foreach (Mission mission in missions)
        {
            mission.currentAmount = 0;
            mission.isCompleted = false;
            //missionUI._nextScene = 6;
        }
    }

    public void ResetMissionAmount(int index)
    {
        missions[index].currentAmount = 0;
    }
}