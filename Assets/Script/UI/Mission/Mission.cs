// Serializable class for a mission
[System.Serializable]
public class Mission
{
    public string title; // The title of the mission
    public string description; // A description providing details about the mission
    public int goalAmount; // The target number required to complete the mission
    public int currentAmount; // The current progress of the mission
    public int reward; // The coin reward for completing the mission
    public int expReward; // The experience points rewarded for completing the mission
    public bool isCompleted; // Flag indicating if the mission is completed or not

    // This method checks if the mission has been completed by comparing current progress with the goal
    public void CheckCompletion()
    {
        if (currentAmount >= goalAmount) // If the current progress has reached or exceeded the goal
        {
            CompleteMission(); // Call the method to complete the mission
        }
    }

    // This method is called when the mission is completed
    private void CompleteMission()
    {
        isCompleted = true; // Set the mission status as completed

        // Add the reward (coins) to the player's inventory
        Inventory.FindObjectOfType<Inventory>().AddCoin(reward);

        // Add the experience reward to the player's stats
        PlayerStats.FindObjectOfType<PlayerStats>().AddExperience(expReward);
    }
}