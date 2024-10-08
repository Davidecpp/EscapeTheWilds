using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    // Value Txts
    public TextMeshProUGUI levelValueTxt;
    public TextMeshProUGUI healthValueTxt;
    public TextMeshProUGUI speedValueTxt;
    public TextMeshProUGUI jumpValueTxt;
    
    private PlayerStats _stats;
    // Start is called before the first frame update
    void Start()
    {
        _stats = FindObjectOfType<PlayerStats>();
        SetStats();
    }
    
    // Set player stats
    private void SetStats()
    {
        speedValueTxt.text = ""+_stats.runSpeed;
        jumpValueTxt.text = ""+ _stats.jumpHeight;
        healthValueTxt.text = ""+_stats.health;
        levelValueTxt.text = ""+_stats.level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
