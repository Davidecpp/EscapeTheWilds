using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerStats.ReduceHealth(1);
            Destroy(gameObject);
        }
    }
}