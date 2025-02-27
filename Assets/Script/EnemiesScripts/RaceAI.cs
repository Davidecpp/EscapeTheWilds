using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaceAI : MonoBehaviour
{
    [SerializeField] private List<Transform> _goals;

    public float goalReachedThreshold = 1.0f; // Distanza per considerare l'obiettivo raggiunto

    private NavMeshAgent _agent;
    private int _walkableAreaMask;

    /// <summary> If true, the AI will search an Object in the scene with tag "Goal".</summary>
    public bool searchForGoal = true;

    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;
    private float lastJumpTime;
    private int currentGoalIndex = 0; // Indice dell'obiettivo corrente

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        // Set auto-braking and auto-repath to true
        _agent.autoBraking = true;
        _agent.autoRepath = true;
        _walkableAreaMask = NavMesh.GetAreaFromName("Walkable");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing.");
            return;
        }

        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not placed on a NavMesh.");
            return;
        }

          // GOAL SEARCH
        if (searchForGoal && (_goals == null || _goals.Count == 0))
        {
            Debug.Log("Searching for goals");
            // try get _target from tag in the scene
            GameObject[] targetObject = GameObject.FindGameObjectsWithTag("Goal");
            foreach (GameObject goal in targetObject)
            {
                _goals.Add(goal.transform);
            }
        }

        if (_goals.Count > 0)
        {
            _agent.SetDestination(_goals[currentGoalIndex].position);
        }
        else
        {
            Debug.LogError("No goals set for RaceAI.");
        }
    }

    void Update()
    {
        if (_agent == null || !_agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not on the NavMesh in Update.");
            return;
        }

        // Controlla se l'agente ha raggiunto l'obiettivo corrente
        if (_goals.Count > 0 && Vector3.Distance(transform.position, _goals[currentGoalIndex].position) < goalReachedThreshold)
        {
            SwitchGoal(); 
        }
    }

    // Cambia l'obiettivo al prossimo della lista (ciclo tra 3)
    private void SwitchGoal()
    {
        currentGoalIndex = (currentGoalIndex + 1) % _goals.Count; 
        _agent.SetDestination(_goals[currentGoalIndex].position);
    }

    void OnDrawGizmos()
    {
        // Disegna il percorso del NavMeshAgent
        if (_agent != null && _agent.path != null)
        {
            Gizmos.color = Color.green;
            var path = _agent.path;
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

     public bool IsOnWalkable(Vector3 objPosition, float tollerance = 0.1f)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(objPosition, out hit, tollerance, _walkableAreaMask);
        return hit.hit;
    }

}
