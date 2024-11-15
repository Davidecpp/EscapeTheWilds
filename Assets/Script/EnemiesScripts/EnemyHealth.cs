using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100.0f; // Maximum health of the enemy
    private float _currentHealth; // Current health of the enemy, updated when damage is taken

    [Header("UI")]
    [SerializeField] private Slider _healthSlider; // Reference to the health bar slider UI

    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] itemsToSpawn; // Array of items to spawn upon enemy death
    [SerializeField] private int numberOfItemsToSpawn = 3; // Number of items to spawn when the enemy dies
    [SerializeField] private float spawnHeightOffset = 1.0f; // Offset from the ground for item spawn position
    [SerializeField] private float raycastDistance = 10.0f; // Distance for the raycast to check ground level

    [Header("Damage Effects")]
    [SerializeField] private GameObject hitParticles; // Particles effect to instantiate when the enemy takes damage
    private AudioSource _audioSource; // Audio source for playing damage sounds

    // References to other game components
    private PlayerStats _playerStats;
    private EnemySpawner _enemySpawner;
    private MissionManager _missionManager;

    private void Awake()
    {
        // Initialize component and object references
        _audioSource = GetComponent<AudioSource>();
        _playerStats = FindObjectOfType<PlayerStats>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _missionManager = FindObjectOfType<MissionManager>();
    }

    private void Start()
    {
        _currentHealth = maxHealth; // Set current health to max at the start
        UpdateHealthBar(); // Update the UI health bar to show full health initially
    }

    // Reduces health by the specified damage amount
    public void TakeDamage(float damageAmount)
    {
        if (damageAmount <= 0 || _currentHealth <= 0) return; // Ignore if damage is zero or enemy is already dead

        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0); // Reduce health, clamp at 0
        SpawnHitEffect(); // Show damage effect
        UpdateHealthBar(); // Update UI health bar

        // Play damage sound
        if (_audioSource != null)
            _audioSource.Play();

        // Check if health has dropped to zero and trigger death if so
        if (_currentHealth <= 0)
            Die();
    }

    // Handles enemy death
    private void Die()
    {
        // Add experience to the player and update enemy spawn statistics
        _playerStats?.AddExperience(30);
        _enemySpawner.killed++;
        
        // If not in arena add progress to the mission
        if (!GameManager.Instance.arenaMode)
        {
            _missionManager?.AddProgress("Tutorial 3", 1); // Update mission progress
        }
        

        SpawnItems(); // Spawn loot items
        Destroy(gameObject); 
    }

    // Spawns loot items at enemy's death location
    private void SpawnItems()
    {
        if (itemsToSpawn.Length == 0) return; // No items to spawn, exit early

        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            // Select a random item from the array
            GameObject itemToSpawn = itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];
            // Initial spawn position above the enemy's position
            Vector3 spawnPosition = transform.position + Vector3.up * spawnHeightOffset;

            // Adjust spawn position based on ground level using raycast
            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, raycastDistance))
                spawnPosition.y = hit.point.y + spawnHeightOffset;

            Instantiate(itemToSpawn, spawnPosition, Quaternion.identity); // Spawn the item
        }
    }

    // Instantiates hit particle effects at enemy's position when damaged
    private void SpawnHitEffect()
    {
        if (hitParticles == null) return; // Exit if no hit particles are assigned

        Vector3 effectPosition = transform.position + Vector3.up; // Position the effect above the enemy
        GameObject particles = Instantiate(hitParticles, effectPosition, Quaternion.identity); // Spawn particles
        Destroy(particles, 0.3f); // Destroy particles after a short duration
    }

    // Updates the health bar UI based on current health
    private void UpdateHealthBar()
    {
        if (_healthSlider != null)
            _healthSlider.value = _currentHealth / maxHealth; // Set slider value as a percentage of max health
    }
}
