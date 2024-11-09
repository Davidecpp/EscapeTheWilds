using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    // Enemy Health
    public float maxHealth = 100.0f;
    public float currentHealth;
    
    private PlayerStats _playerStats;
    [SerializeField] private Slider _slider;
    private ArenaManager _arenaManager;
    private EnemySpawner _enemySpawner;
    
    // Spawn loot
    public GameObject[] itemsToSpawn; 
    public int numberOfItemsToSpawn = 3; 
    public float spawnHeightOffset = 1.0f; // Vertical offset for objs
    public float raycastDistance = 10.0f; // Raycast distance from the ground
    
    // Damage Effects
    public GameObject hitParticles;
    private AudioSource _audioSource;
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        _audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        _arenaManager = FindObjectOfType<ArenaManager>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();

    }
    
    // Take damage
    public void TakeDamage(float amount)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        currentHealth -= amount;
        UpdateHealthBar();
        
        // Hit particles
        GameObject particles;
        particles = Instantiate(hitParticles, pos, transform.rotation);
        Destroy(particles, 0.3f);
        
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        
        Debug.Log("Enemy took damage: " + amount + ", current health: " + currentHealth); 
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    // Make the enemy disappear and call SpawnItems()
    private void Die()
    {
        Debug.Log("Enemy died."); 
        if (_playerStats != null)
        {
            _playerStats.AddExperience(30);
        }
        SpawnItems();
        _enemySpawner.killed++;
        Destroy(gameObject);
    }
    
    // Spawn items
    private void SpawnItems()
    {
        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            // Choose random object from array
            GameObject itemToSpawn = itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];

            // Obj spawn position
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += spawnHeightOffset; 

            // Raycast to verify the object is above the ground
            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, raycastDistance))
            {
                spawnPosition.y = hit.point.y + spawnHeightOffset;
            }
            else
            {
                spawnPosition.y += spawnHeightOffset;
            }
            Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private void UpdateHealthBar()
    {
        _slider.value = currentHealth / maxHealth;
    }
}
