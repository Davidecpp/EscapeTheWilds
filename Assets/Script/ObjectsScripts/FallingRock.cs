using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private Rigidbody rb; // Rigidbody for controlling rock's physics
    private bool hasFallen = false; // Flag to check if the rock has already fallen
    [SerializeField] private ParticleSystem particles; // Particle system to spawn on impact
    private PlayerStats _playerStats; // Reference to the player's stats for applying damage

    void Start()
    {
        // Find and assign the PlayerStats component in the scene
        _playerStats = FindObjectOfType<PlayerStats>();
        // Get the Rigidbody and set it to kinematic so it doesn't fall immediately
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 
    }
    
    // Trigger the rock to fall
    public void DropRock()
    {
        rb.isKinematic = false; // Enable physics
        hasFallen = true; // Mark rock as fallen
    }
    
    // Apply damage to the player if hit by the rock
    void OnCollisionEnter(Collision collision)
    {
        if (hasFallen) // Only apply effects if the rock has fallen
        {
            Instantiate(particles, transform.position, transform.rotation); // Spawn particle effects

            if (collision.gameObject.CompareTag("Player"))
            {
                // Ensure _playerStats is assigned
                if (_playerStats == null)
                {
                    _playerStats = FindObjectOfType<PlayerStats>();
                    if (_playerStats == null)
                    {
                        Debug.LogError("PlayerStats not found when rock hit the player.");
                        return;
                    }
                }

                _playerStats.ReduceHealth(2); // Reduce player's health
            }
        }
    }

    // Trigger the rock to fall when the player enters the area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DropRock(); // Call the function to drop the rock
        }
    }
}
