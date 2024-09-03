using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] public float forceAmount = 10f; // La forza con cui la palla viene spinta

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se la collisione è avvenuta con il giocatore
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collisione con il giocatore!");

            // Calcola la direzione in cui spingere la palla (lontano dal giocatore)
            Vector3 forceDirection = transform.position - collision.transform.position;
            forceDirection.y = 0; // Ignora l'asse Y per evitare che la palla voli in aria
            forceDirection.Normalize(); // Normalizza la direzione per ottenere un vettore unitario

            // Applica una forza alla palla nella direzione calcolata
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(forceDirection * forceAmount, ForceMode.Impulse);

            Debug.Log("Forza applicata: " + forceDirection * forceAmount);
        }
    }
}