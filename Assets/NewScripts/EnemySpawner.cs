using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Enemy spawn
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int spawned = 0;
    public int killed = 0;

    void Start()
    {
        SpawnEnemy(3, enemyPrefab);
    }

    private void Update()
    {
        Debug.Log("Spawned " + spawned + " enemies");
        Debug.Log("Killed " + killed + " enemies");
        NextWave();
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
        spawned += x;
    }

    private void NextWave()
    {
        if (killed == spawned)
        {
            SpawnEnemy(2, enemyPrefab);
        }
    }
}