using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    // Round
    public int round = 1;
    public TextMeshProUGUI roundTxt;
    
    // Enemies
    public int deadCount = 0;
    private bool _extraEnemySpawned = false; // State variable for extra enemy spawn
    private EnemySpawner _enemySpawner;
    public GameObject snakeEnemy;
    public GameObject birdEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        roundTxt.text = "Round " + round;
    }
    
    // Spawn enemies when others are defeated 
    public void SpawnRoundEnemies()
    {
        if (deadCount == 1 && round < 3)
        {
            NextRound();
            _enemySpawner.SpawnEnemy(1, snakeEnemy);
        }
        else if(deadCount == 2 && round >= 3 && round < 7)
        {
            NextRound();
            _enemySpawner.SpawnEnemy(2, snakeEnemy);
        }
        else if (deadCount == 3 && round >= 7 && round < 10)
        {
            NextRound();
            _enemySpawner.SpawnEnemy(3, snakeEnemy);
        }
        if ((round == 3 || round == 7) && !_extraEnemySpawned)
        {
            _enemySpawner.SpawnEnemy(1, snakeEnemy);
            _extraEnemySpawned = true;
        }
        if (round == 10 && !_extraEnemySpawned)
        {
            _enemySpawner.SpawnEnemy(1,  birdEnemy);
            _extraEnemySpawned = true;
        }
    }
    // Set variables for next round
    private void NextRound()
    {
        round++;
        deadCount = 0;
        _extraEnemySpawned = false;
    }
}
