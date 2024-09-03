using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float currentHealth;
    private PlayerStats _playerStats;

    [SerializeField] private Slider _slider;

    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthBar();
        
        Debug.Log("Enemy took damage: " + amount + ", current health: " + currentHealth); 
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died."); 
        _playerStats.AddExperience(30);
        Destroy(gameObject);
    }

    public void UpdateHealthBar()
    {
        _slider.value = currentHealth / maxHealth;
    }
}