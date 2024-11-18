using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10.0f; // Projectile damage
    public bool cannon; // Flag for cannon type

    // Particles
    public GameObject hitParticles; // Normal hit particles
    public GameObject fireParticles; // Fire hit particles

    private PlayerStats playerStats;  // Cache PlayerStats for efficiency

    private void Start()
    {
        // Cache PlayerStats for efficiency
        playerStats = FindObjectOfType<PlayerStats>();
    }
    
    // When projectile collides
    private void OnCollisionEnter(Collision collision)
    {
        GameObject particles;

        // Check if projectile is heated or cannon-based to determine the particle effect
        if (playerStats.heated || cannon)
        {
            particles = Instantiate(fireParticles, transform.position, transform.rotation); // Instantiate fire particles
            Destroy(particles, 6.5f);  // Set this time to be adjustable in the inspector
        }
        else
        {
            particles = Instantiate(hitParticles, transform.position, transform.rotation); // Instantiate normal particles
            Destroy(particles, 1f);  // Set this time to be adjustable in the inspector
        }

        // Damage the enemy if the collision is with an "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Damage the enemy
            }
            Destroy(gameObject);  // Destroy the projectile immediately after hitting the enemy
        }
        else
        {
            Destroy(gameObject, 1f);  // Destroy projectile after 1 second if it's not an enemy
        }
    }
}