using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    // Stats
    private int health;
    public int maxHealth = 5;
    public float moveSpeed = 6.0f;
    public float jumpHeight = 15.0f;
    public float runSpeed = 10.0f;
    public float damage = 10.0f;
    public float exp = 0.0f;
    public float nextLevel = 100;
    public float level = 1.0f;

    private CanvasManager _canvas;
    private GameManager _gameManager;

    private void Start()
    {
        _canvas = FindObjectOfType<CanvasManager>();
        _gameManager = FindObjectOfType<GameManager>();
        health = maxHealth;
        _canvas.UpdateHearts();
    }

    // Add experience to the player
    public void AddExperience(float gained)
    {
        exp += gained;
        if (exp >= nextLevel)
        {
            UpgradeStats();
            exp -= nextLevel;
            nextLevel += 50;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void ReduceHealth(int amount)
    {
        if (health > 0)
        {
            health-= amount;
        }
    }
    public float GetExperience()
    {
        return exp;
    }
    // Add +1 health
    public void AddHeart()
    {
        if (health < maxHealth)
        {
            health++;
            _canvas.UpdateHearts();
            _gameManager.PauseGame();
        }
    }
    
    // Upgrade stats
    private void UpgradeStats()
    {
        level++;
        damage++;
        maxHealth++;
        runSpeed++;
        jumpHeight++;
    }
}