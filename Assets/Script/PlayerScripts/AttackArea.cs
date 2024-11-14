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
    
    public GameObject hitParticles;

    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Attack());
    }

    // When enemy enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collided");
            enemyInRange = other; 
        }
    }

    // When enemy exit the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy exited");
            enemyInRange = null; 
        }
    }

    IEnumerator Attack()
    {
        if (enemyInRange != null && Input.GetMouseButtonDown(0))
        {
            yield return new WaitForSeconds(0.3f);
            Debug.Log("Enemy damage");
            EnemyHealth enemyHealth = enemyInRange.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null && enemyInRange)
            {
                enemyHealth.TakeDamage(_playerStats.GetDamage()); 
            }
        }
    }
}
