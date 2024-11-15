using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Player attributes that can be set in the Unity Inspector
    [SerializeField] private int maxHealth = 5;       // Maximum health of the player
    [SerializeField] private float moveSpeed = 6.0f;  // Base movement speed
    [SerializeField] private float jumpHeight = 15.0f; // Jump height
    [SerializeField] private float runSpeed = 10.0f;  // Running speed
    [SerializeField] private float damage = 10.0f;    // Damage dealt by the player
    [SerializeField] private float nextLevelExp = 100; // Experience required for the next level

    // Private variables for managing player stats
    private int _health;       // Current health
    private float _exp;        // Current experience points
    private int _level = 1;    // Current level of the player

    // Status flags
    public bool heated;        // Indicates if the player is in a "heated" state (e.g., fire effect)

    // Reference to the CanvasManager for updating the UI
    private CanvasManager _canvas;

    private void Start()
    {
        // Initialize the CanvasManager and set up player references
        _canvas = CanvasManager.Instance;
        if (_canvas != null)
        {
            _canvas.SetPlayerReference(this);
            _health = maxHealth;       // Set current health to the maximum
            _canvas.UpdateHearts();    // Update the UI hearts display
        }
        else
        {
            Debug.LogError("CanvasManager not found."); // Log an error if CanvasManager is not found
        }
        
        // Load saved experience and level from PlayerPrefs
        _exp = PlayerPrefs.GetFloat("PlayerExp", 0); // Default to 0 if no saved data
        _level = PlayerPrefs.GetInt("PlayerLevel", 1);

        // Check for any initial dialogues to display
        var dialogue = FindObjectOfType<Dialogue>();
        dialogue.CheckInitialDialogue();
    }
    
    private void Update()
    {
        // Check if the player has died
        Die();
        
        // Handle the "heated" state and turn it off after a delay
        if (heated)
        {
            StartCoroutine(FlameOff(5.0f));
        }
    }

    // Add experience points to the player
    public void AddExperience(float amount)
    {
        _exp += amount; // Increase current experience
        if (_exp >= nextLevelExp) // Check if enough experience is gained for a level-up
        {
            LevelUp();
            _exp -= nextLevelExp;        // Subtract the experience used for leveling up
            nextLevelExp *= 1.5f;        // Increase the experience required for the next level
        }
        
        // Save the updated experience and level
        PlayerPrefs.SetFloat("PlayerExp", _exp);
        PlayerPrefs.SetInt("PlayerLevel", _level);
        PlayerPrefs.Save();
    }

    // Decrease health by a specified amount
    public void ReduceHealth(int amount)
    {
        if (_health <= 0) return; // Ignore if health is already 0
        
        _health -= amount;       // Subtract health
        _canvas?.UpdateHearts(); // Update the hearts UI
        StartCoroutine(_canvas?.FlashRed()); // Flash red effect on damage
    }

    // Add one health point, if under the maximum
    public void AddHeart()
    {
        if (_health < maxHealth)
        {
            _health++;
            _canvas?.UpdateHearts(); // Update the hearts UI
        }
    }
    
    // Handle player death
    private void Die()
    {
        if (_health <= 0)
        {
            GameManager.Instance.GameOver(); // Trigger GameOver sequence
        }
    }

    // Level up the player and improve stats
    private void LevelUp()
    {
        _level++;           // Increase level
        damage++;           // Increase damage
        maxHealth++;        // Increase maximum health
        runSpeed++;         // Increase running speed
        jumpHeight++;       // Increase jump height
        
        // Save the updated level
        PlayerPrefs.SetInt("PlayerLevel", _level);
        PlayerPrefs.Save();
    }
    
    // Turn off the "heated" state after a delay
    public IEnumerator FlameOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        heated = false; // Reset heated state
    }
    
    // Getter methods to access private fields
    public int GetMaxHealth() => maxHealth;
    public float GetMoveSpeed() => moveSpeed;
    public float GetJumpHeight() => jumpHeight;
    public float GetRunSpeed() => runSpeed;
    public float GetDamage() => damage;
    public float GetExperience() => _exp;
    public float GetNextLevelExp() => nextLevelExp;
    public float GetLevel() => _level;
    public int GetHealth() => _health;
    
    // Setter methods to modify private fields
    public void SetHealth(int health) => _health = health;
    public void SetMaxHealth(int maxHealth) => this.maxHealth = maxHealth;
    public void SetLevel(int level) => _level = level;
    public void SetExperience(float exp) => _exp = exp;
    public void SetNextLevelExp(float nextLevelExp) => this.nextLevelExp = nextLevelExp;
    public void SetMoveSpeed(float moveSpeed) => this.moveSpeed = moveSpeed;
    public void SetJumpHeight(float jumpHeight) => this.jumpHeight = jumpHeight;
    public void SetRunSpeed(float runSpeed) => this.runSpeed = runSpeed;
    public void SetDamage(float damage) => this.damage = damage;
}
