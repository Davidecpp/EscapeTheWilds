using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    private int _health;
    private int _maxHealth;
    
    private int _speed;
    private int _maxSpeed;
    
    private int _jump;
    private int _maxJump;
    
    public Transform healthContainer;
    public Transform speedContainer;
    public Transform jumpContainer;
    private List<RawImage> healthBar = new List<RawImage>();
    private List<RawImage> speedBar = new List<RawImage>();
    private List<RawImage> jumpBar = new List<RawImage>();
    public RawImage barPrefab;
    // Start is called before the first frame update
    void Start()
    {
        DefaultStats();
        UpdateAllStats();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAllStats();
    }

    private void DefaultStats()
    {
        _health = 1;
        _speed = 1;
        _jump = 1;

        _maxHealth = 5;
        _maxSpeed = 5;
        _maxJump = 5;
    }

    private void UpdateAllStats()
    {
        UpdateStat(healthBar, _health, healthContainer);
        UpdateStat(speedBar, _speed, speedContainer);
        UpdateStat(jumpBar, _jump, jumpContainer);
    }
    
    private void UpdateStat(List<RawImage> statBar, int stat, Transform container)
    {
        while (statBar.Count < stat)
        {
            RawImage barImage = Instantiate(barPrefab, container);
            statBar.Add(barImage);
        }

        while (statBar.Count > stat)
        {
            RawImage barImage = statBar[statBar.Count - 1];
            statBar.Remove(barImage);
            Destroy(barImage.gameObject);
        }

        for (int i = 0; i < statBar.Count; i++)
        {
            statBar[i].gameObject.SetActive(i < stat);
        }
    }

    public void UpgradeHealth()
    {
        Debug.Log("Click");
        Debug.Log("Health:" + _health);
        if (_health < _maxHealth)
        {
            _health++;
        }
    }
    public void UpgradeSpeed()
    {
        Debug.Log("Click");
        Debug.Log("Speed:" + _speed);
        if (_speed < _maxSpeed)
        {
            _speed++;
        }
    }
    public void UpgradeJump()
    {
        Debug.Log("Click");
        Debug.Log("Jump:" + _jump);
        if (_jump < _maxJump)
        {
            _jump++;
        }
    }
}
