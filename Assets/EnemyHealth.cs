using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float currentHealth;
    private PlayerStats _playerStats;

    [SerializeField] private Slider _slider;

    public GameObject[] itemsToSpawn; // Array di prefab degli oggetti da spawnare
    public int numberOfItemsToSpawn = 3; // Numero di oggetti da spawnare
    public float spawnHeightOffset = 1.0f; // Offset verticale per sollevare gli oggetti
    public float raycastDistance = 10.0f; // Distanza del Raycast per verificare il suolo
    
    public GameObject hitParticles;

    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        currentHealth -= amount;
        UpdateHealthBar();
        Instantiate(hitParticles, pos, transform.rotation);
        
        Debug.Log("Enemy took damage: " + amount + ", current health: " + currentHealth); 
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died."); 
        if (_playerStats != null)
        {
            _playerStats.AddExperience(30);
        }
        SpawnItems();
        Destroy(gameObject);
    }

    private void SpawnItems()
    {
        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            // Scegli un oggetto casuale dall'array
            GameObject itemToSpawn = itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];

            // Calcola la posizione di spawn con offset
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += spawnHeightOffset; // Aggiungi l'offset verticale

            // Usa un Raycast per verificare se il punto di spawn è sopra il terreno
            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, raycastDistance))
            {
                // Se il Raycast colpisce qualcosa, posiziona l'oggetto sopra il terreno
                spawnPosition.y = hit.point.y + spawnHeightOffset;
            }
            else
            {
                // Se non colpisce nulla, usa la posizione originale con l'offset
                spawnPosition.y += spawnHeightOffset;
            }

            // Istanziamo l'oggetto nella posizione calcolata
            Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    public void UpdateHealthBar()
    {
        _slider.value = currentHealth / maxHealth;
    }
}
