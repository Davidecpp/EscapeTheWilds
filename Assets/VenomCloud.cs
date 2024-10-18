using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomCloud : MonoBehaviour
{
    public float damagePerSecond = 10f;  // Quantità di danno inflitta al secondo
    public float slowAmount = 0.5f;      // Percentuale di rallentamento (es. 0.5 = 50%)
    public float duration = 5f;          // Durata totale della nube
    public ParticleSystem venomParticles; // Assegna il Particle System della nube

    private void Start()
    {
        // Distruggi l'oggetto dopo che la nube ha esaurito la sua durata
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica se il nemico è nella nube
        if (other.CompareTag("Enemy")) // Assicurati che i nemici abbiano il tag "Enemy"
        {
            // Applica danno continuo
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }

            /*// Rallenta il nemico
            EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.speed *= slowAmount; // Riduce la velocità
            }*/
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        // Ripristina la velocità del nemico quando esce dalla nube
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.speed /= slowAmount; // Ripristina la velocità originale
            }
        }
    }*/
}

