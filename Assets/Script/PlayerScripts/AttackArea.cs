using System.Collections;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private PlayerStats _playerStats;
    private Collider enemyInRange;
    public GameObject hitParticles;

    void Start()
    {
        _playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if (enemyInRange != null && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy collided");
            enemyInRange = other;
        }
    }

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
        yield return new WaitForSeconds(0.3f);

        if (enemyInRange != null)
        {
            Debug.Log("Enemy damage");
            EnemyHealth enemyHealth = enemyInRange.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(_playerStats.GetDamage());
            }
        }
    }
}