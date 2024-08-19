using UnityEngine;

public class NewProj : MonoBehaviour
{
    public float damage = 10.0f; 
    public GameObject hitParticles; 

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile hit: " + collision.gameObject.name); 

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit an enemy: " + collision.gameObject.name); 
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // hit particles
            Instantiate(hitParticles, transform.position, transform.rotation);

            Destroy(gameObject); 
        }
        else
        {
            Destroy(gameObject, 2.0f);
        }
    }
}