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
        kills = FindObjectOfType<EnemySpawner>().killed;
        // Update round txt
        roundTxt.gameObject.SetActive(GameManager.Instance.arenaMode); // set round txt active if arenaMode is  true
        killsTxt.gameObject.SetActive(GameManager.Instance.arenaMode); // set round txt active if arenaMode is  true
        roundTxt.text = "Round " + round; // Update text
        killsTxt.text = "Kills " + kills; // Update text
        
    }
}