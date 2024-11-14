using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Player Stats
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float jumpHeight = 15.0f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float nextLevelExp = 100;
    
    private int _health;
    private float _exp;
    private int _level = 1;
    
    // Player status
    public bool heated;

    private CanvasManager _canvas;

    private void Start()
    {
        _canvas = CanvasManager.Instance;
        if (_canvas != null)
        {
            _canvas.SetPlayerReference(this);
            _health = maxHealth;
            _canvas.UpdateHearts();
        }
        else
        {
            Debug.LogError("CanvasManager not found.");
        }
        
        // Load stats
        _exp = PlayerPrefs.GetFloat("PlayerExp", 0); // 0 if not saved
        _level = PlayerPrefs.GetInt("PlayerLevel", 1);

        if (GameManager.Instance.currentScene == 4)
        {
            FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Enter the house.","Press WASD to move." });
        }
        
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SaveSystem.SavePlayer(this);
            Debug.Log("Saved");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveSystem.LoadPlayer(this);
        }
        
        Die();
        if (heated)
        {
            StartCoroutine(FlameOff(5.0f));
        }
    }

    public void LoadStats()
    {
        SaveSystem.LoadPlayer(this);
    }

    // Adds experience to the player
    public void AddExperience(float amount)
    {
        _exp += amount;
        if (_exp >= nextLevelExp)
        {
            LevelUp();
            _exp -= nextLevelExp;
            nextLevelExp *= 1.5f;
        }
        
        // Save exp and level through scenes
        PlayerPrefs.SetFloat("PlayerExp", _exp);
        PlayerPrefs.SetInt("PlayerLevel", _level);
        PlayerPrefs.Save();
    }

    // Reduces health by a specific amount
    public void ReduceHealth(int amount)
    {
        if (_health <= 0) return;
        
        _health -= amount;
        _canvas?.UpdateHearts();
        StartCoroutine(_canvas?.FlashRed());
    }

    // Add one health point, if under maxHealth
    public void AddHeart()
    {
        if (_health < maxHealth)
        {
            _health++;
            _canvas?.UpdateHearts();
        }
    }
    
    // Game over
    private void Die()
    {
        if (_health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    // Upgrade stats on level-up
    private void LevelUp()
    {
        _level++;
        damage++;
        maxHealth++;
        runSpeed++;
        jumpHeight++;
        
        // Save new level
        PlayerPrefs.SetInt("PlayerLevel", _level);
        PlayerPrefs.Save();
    }
    
    public IEnumerator FlameOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        heated = false;
        Debug.Log("Finish");
    }
    
    // Accessor methods (getters) to expose private fields
    public int GetMaxHealth() => maxHealth;
    public float GetMoveSpeed() => moveSpeed;
    public float GetJumpHeight() => jumpHeight;
    public float GetRunSpeed() => runSpeed;
    public float GetDamage() => damage;
    public float GetExperience() => _exp;
    public float GetNextLevelExp() => nextLevelExp;
    public float GetLevel() => _level;
    public int GetHealth() => _health;
    
    // Setters
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