using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RaceAI : MonoBehaviour
{
    public Transform[] goals; 
    public float goalReachedThreshold = 1.0f; // Distanza per considerare l'obiettivo raggiunto

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
    }

    // Cambia l'obiettivo al prossimo della lista (ciclo tra 3)
    private void SwitchGoal()
    {
        currentGoalIndex = (currentGoalIndex + 1) % goals.Length; 
        agent.SetDestination(goals[currentGoalIndex].position);
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
    
    // if enemy reach the goal before the player display game over
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal")) // If in contact with tag "Goal"
        {
            GameManager.Instance.gameEnded = true;
        }
    }
}
