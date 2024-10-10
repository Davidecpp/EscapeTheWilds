using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Enemy spawn
    public GameObject enemyPrefab; 
    public Transform[] spawnPoints; 
    public float spawnInterval = 3f; 
    private float _timer;

    private ArenaManager _arenaManager;
    
    private int c = 0;

    void Start()
    {
        _timer = spawnInterval;
        _arenaManager = FindObjectOfType<ArenaManager>();
        SpawnEnemy();
    }

    void Update()
    {
        //ResetTimer();
        _arenaManager.NextRound();
    }

    
    
    // Reset the spawn interval timer
    private void ResetTimer()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            SpawnEnemy();
            _timer = spawnInterval;
        }
    }
    // Spawn enemy in a random spawn point
    public void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}