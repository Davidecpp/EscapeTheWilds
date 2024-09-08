using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private PlayerStats _playerStats;
    
    private bool _isAttacking = false;
    private Collider enemyInRange;

    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInRange != null && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Enemy damage");
            EnemyHealth enemyHealth = enemyInRange.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(_playerStats.damage); 
            }
        }
    }

    // Quando il nemico entra nel collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collided");
            enemyInRange = other; 
        }
    }

    // Quando il nemico esce dal collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy exited");
            enemyInRange = null; 
        }
    }
}
