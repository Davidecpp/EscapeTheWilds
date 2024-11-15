using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// Class to manage the missions in the game
public class MissionManager : MonoBehaviour
{
    public List<Mission> missions; // List of all missions
    private MissionUI _missionUI; // Reference to the MissionUI to update the mission interface
    public int activeMissionIndex = 0; // Index of the currently active mission
    public int indexOffset = 5; // Offset to calculate the correct mission index based on the scene index

    // Initialize the MissionManager and find necessary UI components
    private void Start()
    {
        DontDestroyOnLoad(gameObject); // Ensure this object persists across scenes
        _missionUI = FindObjectOfType<MissionUI>(); // Find the MissionUI in the scene
    }

    // Update is called once per frame
    private void Update()
    {
        // Calculate the active mission index based on the current scene build index
        activeMissionIndex = SceneManager.GetActiveScene().buildIndex - indexOffset;

        // If the mission index is valid, display the mission UI
        if (activeMissionIndex >= 0 && activeMissionIndex < missions.Count)
        {
            _missionUI.missionPanel.SetActive(true); // Show the mission panel
            _missionUI.UpdateUI(); // Update the UI with the current mission data
        }
        else
        {
            _missionUI.missionPanel.SetActive(false); // Hide the mission panel if the index is out of range
            Debug.LogWarning("Active mission index is out of range."); // Log a warning if the index is invalid
        }
    }

    // Method to add progress to a specific mission by title
    public void AddProgress(string missionTitle, int amount)
    {
        // Find the mission by title from the list of missions
        Mission mission = missions.Find(m => m.title == missionTitle);

        // If the mission is found and it is not already completed, add progress
        if (mission != null && !mission.isCompleted)
        {
            mission.currentAmount += amount; // Add the specified amount to the mission's current progress
            mission.CheckCompletion(); // Check if the mission is completed

            // If the mission is completed, reward the player and move to the next mission
            if (mission.isCompleted)
            {
                _missionUI.RewardUI(); // Show reward UI
                activeMissionIndex++; // Move to the next mission
                GameManager.Instance.currentScene++; // Increment the scene index for game progression
            }

            _missionUI.UpdateUI(); // Update the mission UI
        }
        else
        {
            Debug.LogWarning($"Mission '{missionTitle}' not found or already completed."); // Log a warning if the mission is not found or completed
        }
    }

    // Method to reset the status of all missions (progress and completion)
    public void ResetMissionStatus()
    {
        foreach (Mission mission in missions)
        {
            mission.currentAmount = 0; // Reset the current progress
            mission.isCompleted = false; // Set the mission to incomplete
        }
    }

    // Method to reset the progress of a specific mission by its index
    public void ResetMissionAmount(int index)
    {
        // If the index is valid, reset the specific mission's progress
        if (index >= 0 && index < missions.Count)
        {
            missions[index].currentAmount = 0; // Reset the progress of the mission
        }
        else
        {
            Debug.LogWarning("ResetMissionAmount index out of range."); // Log a warning if the index is out of range
        }
    }
}
