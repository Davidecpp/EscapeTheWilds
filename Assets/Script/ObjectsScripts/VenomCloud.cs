
using UnityEngine;

public class VenomCloud : MonoBehaviour
{
    public float damagePerSecond = 10f;  // Amount of damage dealt to enemies per second
    public float slowAmount = 0.5f;      // Percentage to slow enemies (e.g., 0.5 = 50% slower)
    public float duration = 5f;          // Total duration of the venom cloud
    public ParticleSystem venomParticles; // Particle system to visualize the cloud effect

    private void Start()
    {
        // Destroy the venom cloud after its duration expires
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if an enemy is within the cloud
        if (other.CompareTag("Enemy")) // Ensure the object has the "Enemy" tag
        {
            // Continuously apply damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerSecond * Time.deltaTime); // Damage scaled by frame time
            }
        }
    }
}