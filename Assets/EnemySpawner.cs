using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Transform[] spawnPoints; 
    public float spawnInterval = 3f; 

    private float timer;

    void Start()
    {
        timer = spawnInterval; // Inizializza il timer
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval; // Resetta il timer
        }
    }

    void SpawnEnemy()
    {
        // Scegli un punto di spawn casuale tra quelli disponibili
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // Istanziamo il nemico nel punto di spawn scelto
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}