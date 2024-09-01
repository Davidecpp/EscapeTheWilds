using UnityEngine;

public class NewProj : MonoBehaviour
{
    public float damage = 10.0f; 
    public GameObject hitParticles;
    public GameObject fireParticles;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile hit: " + collision.gameObject.name); 
        // hit particles
        if (GameManager.Instance.heated)
        {
            Instantiate(fireParticles, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(hitParticles, transform.position, transform.rotation);
        }

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