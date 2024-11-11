using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RaceAI : MonoBehaviour
{
    public Transform[] goals; 
    public float goalReachedThreshold = 1.0f; // Distanza per considerare l'obiettivo raggiunto
    public float jumpForce = 5f;
    public float jumpDistance = 2f;
    public float jumpCooldown = 1f;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;
    private float lastJumpTime;
    private int currentGoalIndex = 0; // Indice dell'obiettivo corrente

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing.");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not placed on a NavMesh.");
            return;
        }

        if (goals.Length > 0)
        {
            agent.SetDestination(goals[currentGoalIndex].position);
        }
        else
        {
            Debug.LogError("No goals set for RaceAI.");
        }

        lastJumpTime = -jumpCooldown;
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not on the NavMesh in Update.");
            return;
        }

        // Controlla se l'agente ha raggiunto l'obiettivo corrente
        if (goals.Length > 0 && Vector3.Distance(transform.position, goals[currentGoalIndex].position) < goalReachedThreshold)
        {
            SwitchGoal(); 
        }

        CheckForNavMeshObstacle(); // Controlla se ci sono ostacoli e salta
    }

    // Cambia l'obiettivo al prossimo della lista (ciclo tra 3)
    private void SwitchGoal()
    {
        currentGoalIndex = (currentGoalIndex + 1) % goals.Length; 
        agent.SetDestination(goals[currentGoalIndex].position);
        Debug.Log("Switched goal to: " + goals[currentGoalIndex].name);
    }

    // Verifica la presenza di ostacoli sul percorso
    void CheckForNavMeshObstacle()
    {
        if (isJumping || Time.time - lastJumpTime < jumpCooldown)
        {
            return;
        }

        // Controlla se il NavMeshAgent sta cercando di aggirare un NavMeshObstacle
        if (agent.hasPath)
        {
            NavMeshHit hit;
            if (NavMesh.Raycast(agent.transform.position, agent.steeringTarget, out hit, NavMesh.AllAreas))
            {
                if (hit.distance <= jumpDistance && hit.normal.y < 0.1f) // Se rileva un ostacolo vicino
                {
                    Debug.Log("NavMeshObstacle detected. Initiating jump.");
                    StartCoroutine(Jump());
                }
            }
        }
    }

    IEnumerator Jump()
    {
        isJumping = true;
        agent.enabled = false;
        
        Debug.Log("Jumping");
        
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        yield return new WaitForSeconds(jumpCooldown);

        agent.enabled = true;
        isJumping = false;
        lastJumpTime = Time.time;
    }

    void OnDrawGizmos()
    {
        // Disegna il percorso del NavMeshAgent
        if (agent != null && agent.path != null)
        {
            Gizmos.color = Color.green;
            var path = agent.path;
            Vector3 previousCorner = transform.position;

            foreach (var corner in path.corners)
            {
                Gizmos.DrawLine(previousCorner, corner);
                previousCorner = corner;
            }
        }
    }
}
