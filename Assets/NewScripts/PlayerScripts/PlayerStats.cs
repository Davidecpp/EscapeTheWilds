using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    // Stats
    public int health = 3;
    public float moveSpeed = 6.0f;
    public float jumpHeight = 15.0f;
    public float runSpeed = 10.0f;
    public float damage = 10.0f;
    public float exp = 0.0f;
    public float nextLevel = 100;
    public float level = 1.0f;
    
    
    // Add experience to the player
    public void AddExperience(float gained)
    {
        exp += gained;
        if (exp >= nextLevel)
        {
            UpgradeStats();
            exp -= nextLevel;
            nextLevel += 50;
            Debug.Log("Level: " + level);
        }
    }
    public float GetExperience()
    {
        return exp;
    }
    
    // Upgrade stats
    private void UpgradeStats()
    {
        level++;
        damage++;
        health++;
        runSpeed++;
        jumpHeight++;
    }
}