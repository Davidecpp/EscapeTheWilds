using UnityEngine;

public class FallingRock : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;
    [SerializeField] private ParticleSystem particles;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 
    }

    void Update()
    {
        // Puoi usare un trigger o un timer per far cadere il masso.
    }

    public void DropRock()
    {
        rb.isKinematic = false; 
        hasFallen = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasFallen)
        {
            // Aggiungi qui effetti sonori o particelle per l'impatto
            Debug.Log("Il masso ha colpito " + collision.gameObject.name);
            Instantiate(particles, transform.position, transform.rotation);
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.DecreaseHealth();
            }
            //Destroy(gameObject, 2f); 
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DropRock();
        }
    }

}