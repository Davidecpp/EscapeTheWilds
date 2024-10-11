using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Enemy spawn
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; 
    public float spawnInterval = 3f; 
    private float _timer;

    private ArenaManager _arenaManager;

    void Start()
    {
        _timer = spawnInterval;
        _arenaManager = FindObjectOfType<ArenaManager>();
        SpawnEnemy(1, _arenaManager.snakeEnemy);
    }

    void Update()
    {
        //ResetTimer();
        _arenaManager.NextRound();
    }
    /*
    // Reset the spawn interval timer
    private void ResetTimer()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            SpawnEnemy();
            _timer = spawnInterval;
        }
    }*/
    // Spawn x enemies in random spawn points
    public void SpawnEnemy(int x, GameObject enemy)
    {
        for (int i = 0; i < x; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        }
    }
}