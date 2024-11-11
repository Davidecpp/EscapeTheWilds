using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float moveForce = 5f;
    public float wallForce = 10f; // Forza con cui la palla si muoverà
    private Rigidbody rb;
    public GameObject spawn, spawnPlayer, player;

    void Start()
    {
        // Otteniamo il componente Rigidbody associato alla palla
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Controlliamo se la palla è stata toccata dal player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player toccato");
            Vector3 directionAwayFromPlayer = (transform.position - other.transform.position).normalized;
            rb.AddForce(directionAwayFromPlayer * moveForce, ForceMode.Impulse);
        }

        if (other.CompareTag("Goal"))
        {
            Debug.Log("Victory");

            // Fermiamo la palla impostando la velocità del Rigidbody a zero
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            player.GetComponent<Rigidbody>().MovePosition(spawnPlayer.transform.position);

            transform.position = spawn.transform.position;
        }

        // Controlliamo se la palla ha toccato un muro con il tag "JumpPlatform"
        if (other.CompareTag("JumpPlatform"))
        {
            Debug.Log("Muro toccato");

            // Calcoliamo la normale rispetto alla posizione della palla e del muro
            Vector3 normal = (transform.position - other.transform.position).normalized;

            // Riflessione della direzione del movimento
            Vector3 direction = rb.velocity;
            Vector3 reflectedDirection = Vector3.Reflect(direction, normal);

            // Applichiamo la forza nella direzione riflessa
            rb.velocity = reflectedDirection.normalized * wallForce;
        }
    }
}