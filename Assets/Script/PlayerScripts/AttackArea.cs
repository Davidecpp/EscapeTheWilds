using System.Collections;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private PlayerStats _playerStats;        // Cached reference to the PlayerStats
    private Collider enemyInRange;           // Cached reference to the enemy within range
    public GameObject hitParticles;          // Particle effect to instantiate when an enemy is hit

    private bool isAttacking = false;        // Flag to check if the attack is already in progress

    void Start()
    {
        // Cache the PlayerStats reference at the start
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        // Perform the attack if the enemy is in range and the mouse button is clicked
        if (enemyInRange != null && !isAttacking && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When an enemy enters the attack area, set enemyInRange
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset enemyInRange when the enemy exits the attack area
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = null;
        }
    }

    private IEnumerator Attack()
    {
        // Flag to prevent re-triggering the attack during an active coroutine
        isAttacking = true;

        // Wait for a small amount of time (0.3f) to simulate the attack delay
        yield return new WaitForSeconds(0.3f);

        // Only damage the enemy if the enemy is still in range after the delay
        if (enemyInRange != null)
        {
            Debug.Log("Enemy damage");
            EnemyHealth enemyHealth = enemyInRange.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(_playerStats.GetDamage());
                
                // Optionally instantiate hit particles at the enemy's position
                if (hitParticles != null)
                {
                    Instantiate(hitParticles, enemyInRange.transform.position, Quaternion.identity);
                }
            }
        }

        // Reset attack flag after attack is complete
        isAttacking = false;
    }
}
