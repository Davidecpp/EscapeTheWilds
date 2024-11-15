
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    // Public variables to control pendulum speed, swing angle limit, and random starting phase
    public float speed = 1.5f; // Controls how fast the pendulum swings
    public float limit = 75f; // Maximum angle of the pendulum swing
    public bool randomStart = false; // If true, starts the pendulum at a random phase
    private float random = 0; // Holds the random phase offset
    private PlayerStats _playerStats; // Reference to the player's stats for applying damage

    private void Awake()
    {
        // Assign a random phase offset if randomStart is true
        if (randomStart)
        {
            random = Random.Range(0f, 1f);
        }
    }

    private void Start()
    {
        // Find and assign the PlayerStats component in the scene
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        // Simulate pendulum swinging back and forth using sine wave
        float angle = limit * Mathf.Sin((Time.time + random) * speed);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    
    // Detect collision with the player and apply damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Re-check and assign PlayerStats in case it's null
            if (_playerStats == null)
            {
                _playerStats = FindObjectOfType<PlayerStats>();
                if (_playerStats == null)
                {
                    Debug.LogError("PlayerStats not found when pendulum hit the player.");
                    return;
                }
            }
            _playerStats.ReduceHealth(1); // Reduce player's health
        }
    }
}