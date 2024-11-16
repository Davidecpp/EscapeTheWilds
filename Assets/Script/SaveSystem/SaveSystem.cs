using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    // Define the path where the save file will be stored (persistent data path)
    private static readonly string saveFilePath = Application.persistentDataPath + "/playerSave.json";

    // Method to save the player's data to a JSON file
    public static void SavePlayer(PlayerStats playerStats)
    {
        // Find the Inventory object to get the coin count
        var inventory = FindObjectOfType<Inventory>();
        
        // Create a PlayerSaveData object, which holds all necessary player stats
        var saveData = new PlayerSaveData
        {
            health = playerStats.GetHealth(), // Current health of the player
            maxHealth = playerStats.GetMaxHealth(), // Maximum health of the player
            level = (int)playerStats.GetLevel(), // Current level of the player
            experience = playerStats.GetExperience(), // Experience points of the player
            nextLevelExp = playerStats.GetNextLevelExp(), // Experience required for the next level
            moveSpeed = playerStats.GetMoveSpeed(), // Movement speed of the player
            jumpHeight = playerStats.GetJumpHeight(), // Jump height of the player
            runSpeed = playerStats.GetRunSpeed(), // Running speed of the player
            damage = playerStats.GetDamage(), // Damage dealt by the player
            coin = inventory?.GetCoinCount() ?? 0, // Get the coin count from inventory, or set to 0 if inventory is null
            strawberries = inventory?.GetStrawberryCount() ?? 0, // Get the strawberry count from inventory, or set to 0 if inventory is null
            sceneName = SceneManager.GetActiveScene().name, // Store the name of the current scene
            sceneIndex = GameManager.Instance.currentScene // Store the index of the current scene from GameManager
        };

        // Convert the PlayerSaveData object into a JSON string, with indentation for readability
        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData, true));
    }

    // Method to load the player's data from a JSON file
    public static void LoadPlayer(PlayerStats playerStats)
    {
        // Check if the save file exists at the specified path
        if (File.Exists(saveFilePath))
        {
            // Read the content of the save file into a JSON string
            var json = File.ReadAllText(saveFilePath);

            // Deserialize the JSON string into a PlayerSaveData object
            var saveData = JsonUtility.FromJson<PlayerSaveData>(json);

            // Restore the player's stats using the data from the save file
            playerStats.SetHealth(saveData.health); // Set the player's health
            playerStats.SetMaxHealth(saveData.maxHealth); // Set the player's max health
            playerStats.SetLevel(saveData.level); // Set the player's level
            playerStats.SetExperience(saveData.experience); // Set the player's experience
            playerStats.SetNextLevelExp(saveData.nextLevelExp); // Set the experience required for the next level
            playerStats.SetMoveSpeed(saveData.moveSpeed); // Set the player's movement speed
            playerStats.SetJumpHeight(saveData.jumpHeight); // Set the player's jump height
            playerStats.SetRunSpeed(saveData.runSpeed); // Set the player's running speed
            playerStats.SetDamage(saveData.damage); // Set the player's damage
            playerStats.SetStamina(saveData.stamina); // Set the player's stamina

            // If the Inventory object is found, restore the coin and the strawberry count
            var inventory = FindObjectOfType<Inventory>();
            inventory?.SetCoinCount(saveData.coin);
            inventory?.SetStrawberryCount(saveData.strawberries);

            // Reset mission-related data using the saved scene index
            var missionManager = FindObjectOfType<MissionManager>();
            missionManager?.ResetMissionAmount(saveData.sceneIndex - missionManager.indexOffset); // Adjust mission count based on scene
            missionManager?.ResetMissionStatus(); // Reset the mission status

            // Load the scene that was saved in the file
            SceneManager.LoadScene(saveData.sceneName);

            // Update the GameManager with the current scene index
            GameManager.Instance.currentScene = saveData.sceneIndex;
        }
        else
        {
            // Log an error if the save file is not found at the specified path
            Debug.LogError("Save file not found!");
        }
    }
}
