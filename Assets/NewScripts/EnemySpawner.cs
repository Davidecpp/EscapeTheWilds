using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Enemy spawn
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; 

    private ArenaManager _arenaManager;

    void Start()
    {
        _arenaManager = FindObjectOfType<ArenaManager>();
        if (_arenaManager == null || _arenaManager.snakeEnemy == null)
        {
            Debug.LogError("ArenaManager or snakeEnemy not found in the scene or not assigned.");
            return;
        }
        
        SpawnEnemy(1, _arenaManager.snakeEnemy);
    }
    
    // Spawn x enemies in random spawn points
    public void SpawnEnemy(int x, GameObject enemy)
    {
        for (int i = 0; i < x; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        }
        Debug.Log("Spawned " + x + " enemies");
    }
}