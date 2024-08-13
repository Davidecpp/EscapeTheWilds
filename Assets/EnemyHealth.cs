using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy took damage: " + amount + ", current health: " + currentHealth); // Debug log for damage

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle enemy death (e.g., play animation, destroy object, etc.)
        Debug.Log("Enemy died."); // Debug log for death
        Destroy(gameObject);
    }
}