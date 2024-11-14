using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    // Define the path where the save file will be stored
    private static readonly string saveFilePath = Application.persistentDataPath + "/playerSave.json";

    // Saves the player's data to a JSON file
    public static void SavePlayer(PlayerStats playerStats)
    {
        Debug.Log("Saving file at: " + saveFilePath);
        
        var inventory = FindObjectOfType<Inventory>();
        
        // Create a PlayerSaveData object containing the player's stats
        var saveData = new PlayerSaveData
        {
            health = playerStats.GetHealth(),
            maxHealth = playerStats.GetMaxHealth(),
            level = (int)playerStats.GetLevel(),
            experience = playerStats.GetExperience(),
            nextLevelExp = playerStats.GetNextLevelExp(),
            moveSpeed = playerStats.GetMoveSpeed(),
            jumpHeight = playerStats.GetJumpHeight(),
            runSpeed = playerStats.GetRunSpeed(),
            damage = playerStats.GetDamage(),
            coin = inventory?.GetCoinCount() ?? 0, // Get coins from inventory or set to 0 if inventory is null
            sceneName = SceneManager.GetActiveScene().name, // Current scene name
            sceneIndex = GameManager.Instance.currentScene // Current scene index from GameManager
        };

        // Convert the saveData object to a JSON string
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
        Debug.Log("Player saved.");
    }

    // Loads the player's data from a JSON file
    public static void LoadPlayer(PlayerStats playerStats)
    {
        // Check if the save file exists
        if (File.Exists(saveFilePath))
        {
            // Read JSON data from the file
            var json = File.ReadAllText(saveFilePath);
            var saveData = JsonUtility.FromJson<PlayerSaveData>(json);

            // Set the player's stats using the loaded data
            playerStats.SetHealth(saveData.health);
            playerStats.SetMaxHealth(saveData.maxHealth);
            playerStats.SetLevel(saveData.level);
            playerStats.SetExperience(saveData.experience);
            playerStats.SetNextLevelExp(saveData.nextLevelExp);
            playerStats.SetMoveSpeed(saveData.moveSpeed);
            playerStats.SetJumpHeight(saveData.jumpHeight);
            playerStats.SetRunSpeed(saveData.runSpeed);
            playerStats.SetDamage(saveData.damage);

            // Load Inventory and Mission data if the objects are found
            var inventory = FindObjectOfType<Inventory>();
            inventory?.SetCoinCount(saveData.coin);

            var missionManager = FindObjectOfType<MissionManager>();
            missionManager?.ResetMissionAmount(saveData.sceneIndex - 5); // Adjust mission count based on scene
            missionManager?.ResetMissionStatus();

            var missionUI = FindObjectOfType<MissionUI>();
            if (missionUI != null) missionUI._nextScene = saveData.sceneIndex + 1; // Prepare the next scene in MissionUI

            // Load the saved scene
            SceneManager.LoadScene(saveData.sceneName);

            // Update the current scene in GameManager
            GameManager.Instance.currentScene = saveData.sceneIndex;

            Debug.Log("Player loaded.");
        }
        else
        {
            Debug.LogError("Save file not found!");
        }
    }
}
