using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    // UI Text elements to display player stats
    public TextMeshProUGUI levelValueTxt;   // Text displaying the player's current level
    public TextMeshProUGUI healthValueTxt;  // Text displaying the player's current health
    public TextMeshProUGUI speedValueTxt;   // Text displaying the player's run speed
    public TextMeshProUGUI jumpValueTxt;    // Text displaying the player's jump height
    public TextMeshProUGUI damageValueTxt;  // Text displaying the player's damage

    private PlayerStats _stats;  // Reference to the PlayerStats script to get player data

    // Start is called before the first frame update
    void Start()
    {
        // Find the PlayerStats object in the scene and assign it to _stats
        _stats = FindObjectOfType<PlayerStats>();
    }

    // Set the player stats to the UI text elements
    private void SetStats()
    {
        // Set the values of the UI elements based on the player's current stats
        speedValueTxt.text = "" + _stats.GetRunSpeed();  // Display run speed
        jumpValueTxt.text = "" + _stats.GetJumpHeight(); // Display jump height
        healthValueTxt.text = "" + _stats.GetHealth() + "/" + _stats.GetMaxHealth(); // Display current health / max health
        levelValueTxt.text = "" + _stats.GetLevel(); // Display player level
        damageValueTxt.text = "" + _stats.GetDamage(); // Display damage stat
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously update the player stats on the UI
        SetStats();
    }
}