using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Prefab for the enemy that will be spawned
    public GameObject enemyPrefab;
    
    // Array of spawn points where enemies can appear
    public Transform[] spawnPoints;
    
    // Counter for the number of spawned enemies
    public int spawned = 0;
    
    // Counter for the number of enemies killed
    public int killed = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the first enemy (example of starting the spawn with 1 enemy)
        SpawnEnemy(1, enemyPrefab);
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the next wave of enemies should spawn
        NextWave();
    }

    // Spawn x number of enemies at random spawn points
    public void SpawnEnemy(int x, GameObject enemy)
    {
        // Loop to spawn x number of enemies
        for (int i = 0; i < x; i++)
        {
            // Choose a random spawn point from the spawnPoints array
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            
            // Instantiate the enemy at the chosen spawn point position and rotation
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        }
        
        // Increment the count of spawned enemies
        spawned += x;
    }

    // If all spawned enemies are killed, spawn the next wave
    private void NextWave()
    {
        // Check if the number of killed enemies equals the number of spawned enemies
        if (killed == spawned)
        {
            // Spawn the next wave of enemies (2 enemies in this case)
            SpawnEnemy(2, enemyPrefab);
        }
    }
}