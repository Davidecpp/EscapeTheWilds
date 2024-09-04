using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseDistance = 10f;
    private NavMeshAgent _agent;

    [SerializeField] private float timer = 5;
    private float _bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float bulletSpeed;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // Trova il player automaticamente usando il tag "Player"
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

    private void Shoot()
    {
        _bulletTime -= Time.deltaTime;
        if (_bulletTime > 0) return;

        _bulletTime = timer;
        GameObject bullet = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * bulletSpeed);
        Destroy(bullet, 0.5f);
    }

    private void ChasePlayer()
    {
        _agent.isStopped = false;
        _agent.SetDestination(player.position);
    }
}