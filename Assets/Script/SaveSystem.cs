using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    private static string saveFilePath = Application.persistentDataPath + "/playerSave.json";

    // Salva i dati del giocatore
    public static void SavePlayer(PlayerStats playerStats)
    {
        Debug.Log("Salvataggio file su: " + saveFilePath); 
        // Crea un oggetto con i dati del giocatore da salvare
        PlayerSaveData saveData = new PlayerSaveData
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
            coin = FindObjectOfType<Inventory>().GetCoinCount(),
            
            sceneName = SceneManager.GetActiveScene().name
        };

        // Serializza l'oggetto in una stringa JSON
        string json = JsonUtility.ToJson(saveData, true);

        // Salva la stringa JSON su un file
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Giocatore salvato.");
    }

    // Carica i dati del giocatore
    public static void LoadPlayer(PlayerStats playerStats)
    {
        Debug.Log("Loaded");
        if (File.Exists(saveFilePath))
        {
            // Leggi la stringa JSON dal file
            string json = File.ReadAllText(saveFilePath);

            // Deserializza la stringa in un oggetto PlayerSaveData
            PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(json);

            // Imposta i valori del giocatore con i dati caricati
            playerStats.SetHealth(saveData.health);
            playerStats.SetMaxHealth(saveData.maxHealth);
            playerStats.SetLevel(saveData.level);
            playerStats.SetExperience(saveData.experience);
            playerStats.SetNextLevelExp(saveData.nextLevelExp);
            playerStats.SetMoveSpeed(saveData.moveSpeed);
            playerStats.SetJumpHeight(saveData.jumpHeight);
            playerStats.SetRunSpeed(saveData.runSpeed);
            playerStats.SetDamage(saveData.damage);
            FindObjectOfType<Inventory>().AddCoin(saveData.coin);
            
            SceneManager.LoadScene(saveData.sceneName);
            
            string scenePath = "/" + saveData.sceneName;

            // Obtain scene index
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
            GameManager.Instance.currentScene = sceneIndex;

            Debug.Log("Giocatore caricato.");
        }
        else
        {
            Debug.LogError("File di salvataggio non trovato!");
        }
    }
}
