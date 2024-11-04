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
        MissionManager.FindObjectOfType<MissionManager>().activeMissionIndex++;
        Debug.Log("Mission completed: " + title);
        // Puoi aggiungere qui la logica per assegnare la ricompensa
    }
}