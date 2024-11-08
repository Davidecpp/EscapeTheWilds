using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = System.Random;

public class Pendulum : MonoBehaviour
{
    public float speed = 1.5f;
    public float limit = 75f;
    public bool randomStart = false;
    private float random = 0;
    private PlayerStats _playerStats;

    private void Awake()
    {
        if (randomStart)
        {
            random = UnityEngine.Random.Range(0f, 1f);
        }
    }

    private void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        // Swing
        float angle = limit * Mathf.Sin((Time.time + random) * speed);
        transform.localRotation = Quaternion.Euler(0,0,angle);
    }
    
    // Damage player if hit
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision with Player detected.");
            _playerStats.ReduceHealth(1);
        }
    }
}
