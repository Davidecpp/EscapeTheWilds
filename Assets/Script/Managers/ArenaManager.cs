using System;
using TMPro;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    // Round
    public int round = 1;               // current round
    public int kills;
    public TextMeshProUGUI roundTxt;    // Round text
    public TextMeshProUGUI killsTxt;    // Kills text

    // Update is called once per frame
    void Update()
    {
        // Update round txt
        roundTxt.gameObject.SetActive(GameManager.Instance.arenaMode); // set round txt active if arenaMode is  true
        killsTxt.gameObject.SetActive(GameManager.Instance.arenaMode); // set round txt active if arenaMode is  true
        
        Kills();
        
        roundTxt.text = "Round " + round; // Update text
        killsTxt.text = "Kills " + kills; // Update text
    }
    
    // Search for enemy spawner in the scene and get killed amount
    private void Kills()
    {
        var enemy = FindObjectOfType<EnemySpawner>();
        if (enemy != null)
        {
            kills = enemy.killed;
        }
        
    }
}