using UnityEngine;

public class NewProj : MonoBehaviour
{
    public float damage = 10.0f; // Amount of damage this projectile deals

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile hit: " + collision.gameObject.name); // Debug log to check collision

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit an enemy: " + collision.gameObject.name); // Debug log to confirm hitting an enemy
            // Get the enemy's health component and apply damage
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the projectile after it hits the enemy
            Destroy(gameObject);
        }
        else
        {
            // Destroy the projectile if it hits something else
            Destroy(gameObject, 2.0f);
        }
    }
}