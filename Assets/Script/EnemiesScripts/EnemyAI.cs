using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Variables related to chasing the player
    [Header("Chase Settings")]
    [SerializeField] private Transform player;             // Reference to the player’s position
    [SerializeField] private float chaseDistance = 10f;    // Distance within which the enemy will chase the player
    private NavMeshAgent _agent;                           // NavMeshAgent component for enemy navigation

    // Variables related to shooting
    [Header("Shooting Settings")]
    [SerializeField] private GameObject enemyBullet;       // Bullet prefab that the enemy will shoot
    [SerializeField] private Transform spawnPoint;         // Position from which the bullet will be instantiated
    [SerializeField] private float bulletSpeed = 5f;      // Speed of the bullet when fired
    [SerializeField] private float shootInterval = 5f;     // Interval between consecutive shots
    private float _shootCooldown;                          // Timer for managing shooting intervals

    private void Start()
    {
        // Get the NavMeshAgent component and assign the player if not set
        _agent = GetComponent<NavMeshAgent>();
        AssignPlayer();
    }

    private void Update()
    {
        // Exit if no player is assigned
        if (player == null) return;
        
        // Check the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        
        // If player is within chase distance, chase and try to shoot
        if (distanceToPlayer < chaseDistance)
        {
            ChasePlayer();
            TryShoot();
        }
        else
        {
            // Stop the agent if the player is out of range
            _agent.isStopped = true;
        }
    }

    // Assign player if it has not been set in the Inspector
    private void AssignPlayer()
    {
        if (player == null)
        {
            // Find player by tag if not already assigned
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

    // Chase the player using the NavMeshAgent component
    private void ChasePlayer()
    {
        if (!_agent.isStopped)
        {
            // Set the destination to the player's position
            _agent.SetDestination(player.position);
        }
        else
        {
            // Resume movement if the agent is stopped
            _agent.isStopped = false;
        }
    }

    // Attempt to shoot if cooldown period has elapsed
    private void TryShoot()
    {
        // Decrease the cooldown timer over time
        _shootCooldown -= Time.deltaTime;
        
        // If the cooldown timer reaches zero, shoot and reset the timer
        if (_shootCooldown <= 0f)
        {
            Shoot();
            _shootCooldown = shootInterval;
        }
    }

    // Shoot a projectile in the direction the enemy is facing
    private void Shoot()
    {
        // Check if bullet and spawn point are assigned
        if (enemyBullet == null || spawnPoint == null) return;

        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);

        // Apply velocity to the bullet if it has a Rigidbody component
        if (bullet.TryGetComponent(out Rigidbody bulletRb))
        {
            bulletRb.velocity = spawnPoint.forward * bulletSpeed;
        }
        
        // Destroy the bullet after 2 seconds to avoid clutter
        Destroy(bullet, 2f);
    }
}
