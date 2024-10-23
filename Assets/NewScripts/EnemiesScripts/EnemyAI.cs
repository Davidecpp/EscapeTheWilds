using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Variables for following the player
    public Transform player;
    public float chaseDistance = 10f;
    private NavMeshAgent _agent;
    
    // Bullet
    [SerializeField] private float timer = 5;
    private float _bulletTime;
    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float bulletSpeed;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // Find player with tag "Player"
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player not found! Make sure the player object has the 'Player' tag.");
            }
        }
    }

    void Update()
    {
        if (player == null) return;
        
        // If distanceToPlayer < chaseDistance it follows the player
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer < chaseDistance)
        {
            ChasePlayer();
            Shoot();
        }
        else
        {
            _agent.isStopped = true;
        }
    }
    
    // Shoot projectile
    private void Shoot()
    {
        _bulletTime -= Time.deltaTime;
        if (_bulletTime > 0) return;
        _bulletTime = timer;
        
        // Generate the bullet and add the force to it
        GameObject bullet = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * bulletSpeed);
        
        Destroy(bullet, 0.5f);
    }
    
    // Follow player with NevMash agent
    private void ChasePlayer()
    {
        _agent.isStopped = false;
        _agent.SetDestination(player.position);
    }
}