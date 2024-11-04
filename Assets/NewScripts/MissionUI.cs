using UnityEngine;
using TMPro;

public class MissionUI : MonoBehaviour
{
    public TextMeshProUGUI missionText;
    private MissionManager missionManager;

    private void Start()
    {
        missionManager = FindObjectOfType<MissionManager>();
        UpdateUI();
    }

    /*public void UpdateUI()
    {
        missionText.text = "";
        foreach (Mission mission in missionManager.missions)
        {
            missionText.text += $"{mission.title}:\n{mission.description}\n {mission.currentAmount}/{mission.goalAmount}\n";
        }
    }*/
    public void UpdateUI()
    {
        missionText.text = "";
        Mission mission = missionManager.missions[missionManager.activeMissionIndex];
        missionText.text += $"{mission.title}:\n{mission.description}\n {mission.currentAmount}/{mission.goalAmount}\n";
    }
}