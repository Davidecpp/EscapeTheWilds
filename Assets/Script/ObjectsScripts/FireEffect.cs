
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    public float damage = 15.0f; // Damage dealt to enemies
    private PlayerStats _playerStats; // Reference to the player's stats for applying damage

    void Start()
    {
        // Find and assign the PlayerStats component in the scene
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        // Destroy this object after 5.5 seconds
        Destroy(gameObject, 5.5f);
    }

    // Apply effects when colliding with other objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Apply damage to the player
            _playerStats.ReduceHealth(1);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to an enemy if hit
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}