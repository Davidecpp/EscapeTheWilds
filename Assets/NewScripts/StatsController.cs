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
    public TextMeshProUGUI damageValueTxt;
    
    private PlayerStats _stats;
    // Start is called before the first frame update
    void Start()
    {
        _stats = FindObjectOfType<PlayerStats>();
    }
    
    // Set player stats
    private void SetStats()
    {
        speedValueTxt.text = ""+_stats.GetRunSpeed();
        jumpValueTxt.text = ""+ _stats.GetJumpHeight();
        healthValueTxt.text = ""+_stats.GetHealth()+"/"+_stats.GetMaxHealth();
        levelValueTxt.text = ""+_stats.GetLevel();
        damageValueTxt.text = "" + _stats.GetDamage();
    }

    // Update is called once per frame
    void Update()
    {
        SetStats();
    }
}
