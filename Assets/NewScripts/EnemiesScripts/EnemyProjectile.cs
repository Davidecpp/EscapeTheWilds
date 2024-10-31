using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private PlayerStats _player;

    void Start()
    {
        _player = FindObjectOfType<PlayerStats>();
        if (_player == null)
        {
            Debug.LogError("PlayerStats not found in the scene.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            _player?.ReduceHealth(1); // Usa l'operatore nullo condizionale per evitare un altro NullReferenceException
            Destroy(gameObject);
        }
    }
}