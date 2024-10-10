using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    // Round
    public int round = 1;
    private int _currentRound;
    public TextMeshProUGUI roundTxt;

    private EnemySpawner _enemySpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentRound = round;
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        roundTxt.text = "Round " + round;
    }
    public void NextRound()
    {
        if (_currentRound != round)
        {
            _enemySpawner.SpawnEnemy();
            _currentRound = round;
        }
    }
}
