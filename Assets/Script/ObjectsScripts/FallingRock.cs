using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;
    [SerializeField] private ParticleSystem particles;
    private PlayerStats _playerStats;

    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 
    }
    
    // Let the rock fall
    public void DropRock()
    {
        rb.isKinematic = false; 
        hasFallen = true;
    }
    
    // Damage the player if hit
    void OnCollisionEnter(Collision collision)
    {
        if (hasFallen)
        {
            Debug.Log("Rock hit " + collision.gameObject.name);
            Instantiate(particles, transform.position, transform.rotation);
            if (collision.gameObject.CompareTag("Player"))
            {
                _playerStats.ReduceHealth(2);
            }
        }
    }
    // When the player is in the area it activates
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DropRock();
        }
    }

}