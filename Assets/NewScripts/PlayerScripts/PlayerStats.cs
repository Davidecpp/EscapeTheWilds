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
    }
    
    private void Update()
    {
        Die();
    }

    // Accessor methods (getter) to expose private fields
    public int GetMaxHealth() => maxHealth;
    public float GetMoveSpeed() => moveSpeed;
    public float GetJumpHeight() => jumpHeight;
    public float GetRunSpeed() => runSpeed;
    public float GetDamage() => damage;
    public float GetExperience() => _exp;
    public float GetNextLevelExp() => nextLevelExp;
    public float GetLevel() => _level;
    public int GetHealth() => _health;

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
            //_gameManager?.PauseGame();
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
    }
}