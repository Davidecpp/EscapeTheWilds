using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private PlayerStats _playerStats;

    private void Start()
    {
        // Finding the PlayerStats component in the scene and assigning it to the _playerStats variable
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Checking if the object the projectile collided with has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reducing the player's health by 1
            _playerStats.ReduceHealth(1);
            // Destroying the projectile game object
            Destroy(gameObject);
        }
    }
}