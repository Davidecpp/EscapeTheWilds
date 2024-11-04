using UnityEngine;

[System.Serializable]
public class Mission
{
    public string title;
    public string description;
    public int goalAmount; 
    public int currentAmount; 
    public int reward;
    public bool isCompleted;
    
    public void CheckCompletion()
    {
        if (currentAmount >= goalAmount)
        {
            CompleteMission();
        }
    }

    private void CompleteMission()
    {
        isCompleted = true;
        Debug.Log("Mission completed: " + title);
        
        MissionUI.FindObjectOfType<MissionUI>().RewardUI();
        MissionManager.FindObjectOfType<MissionManager>().activeMissionIndex++;
        
        // Add reward
        Inventory.FindObjectOfType<Inventory>().AddCoin(reward);
    }
}