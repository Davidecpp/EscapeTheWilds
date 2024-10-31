using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.ReduceHealth(1);
            Destroy(gameObject);
        }
    }
}