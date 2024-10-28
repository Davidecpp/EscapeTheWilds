using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10.0f;
    public bool cannon;
    
    // Particles
    public GameObject hitParticles;
    public GameObject fireParticles;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject particles;
        Debug.Log("Projectile hit: " + collision.gameObject.name); 
        
        // if heated generate flames
        if (GameManager.Instance.heated || cannon)
        {
            particles = Instantiate(fireParticles, transform.position, transform.rotation);
            Destroy(particles, 6.5f);
        }
        else
        {
            particles = Instantiate(hitParticles, transform.position, transform.rotation);
            Destroy(particles, 1f);
        }
        
        // Damage the enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit an enemy: " + collision.gameObject.name); 
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject); 
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }
}