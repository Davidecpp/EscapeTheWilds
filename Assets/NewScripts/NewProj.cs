using UnityEngine;

public class NewProj : MonoBehaviour
{
    public float damage = 10.0f;
    
    // Particelle
    public GameObject hitParticles;
    public GameObject fireParticles;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject particles;
        Debug.Log("Projectile hit: " + collision.gameObject.name); 
        
        // Se heated genera particelle infuocate altrimenti base
        if (GameManager.Instance.heated)
        {
            particles = Instantiate(fireParticles, transform.position, transform.rotation);
            Destroy(particles, 6.5f);
        }
        else
        {
            particles = Instantiate(hitParticles, transform.position, transform.rotation);
            Destroy(particles, 1f);
        }
        
        // Se colpisce un nemico gli fa danno
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