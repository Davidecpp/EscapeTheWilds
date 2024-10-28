using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Hit Player");
            GameManager.Instance.DecreaseHealth(); 
            Destroy(gameObject);
        }
        
    }
}