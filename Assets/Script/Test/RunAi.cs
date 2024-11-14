using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// This class is responsible for the AI movement using unity NavMesh.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class RunAI : MonoBehaviour
{
    private NavMeshAgent _agent;  
    
    /// <summary> The target that the AI will move towards.</summary>
    [SerializeField] private Transform _goal;

    /// <summary> If true, the AI will search an Object in the scene with tag "Goal".</summary>
    public bool searchForGoal = false;

    private bool isGoalSet = false;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // Set auto-braking and auto-repath to true
        _agent.autoBraking = true;
        _agent.autoRepath = true;


        // GOAL SEARCH
        if (searchForGoal && _goal == null)
        {
            // try get _target from tag in the scene
            GameObject targetObject = GameObject.FindWithTag("Goal");
            _goal = targetObject.transform;
        }

        if (_goal == null)
        {
            Debug.LogError("Goal is null");
        }
        else
        {
            isGoalSet = _agent.SetDestination(_goal.position);
            if (! isGoalSet)
            {
                Debug.LogError("Goal is not set");
            }
            else 
            {
                Debug.Log("Goal set to " + _goal.position);
            }
        }
        
    }

    /// <summary> Smoothness of the AI movement when raycast objects in front. </summary>
    public float movingSmoothness = 0.5f;

    /// <summary> Distance of the raycast in front of the AI. </summary>
    public float raycastDistance = 2f;
    void Update()
    {   
        // AVOID MOVVING ON BORDERS
        NavMeshHit hitForward;
        _agent.Raycast(Vector3.forward, out hitForward);
        

        // if hit something in near front and is outside the navmesh
        bool hitObstacle = hitForward.hit && hitForward.distance < raycastDistance;
        if (hitObstacle && IsOutsideNavMesh(hitForward.position)) 
        {
            addObstaleTo(hitForward.collider.gameObject);
        }
        
        


        // if hit something in near front of the AI
        // bool hitObstacle = hitForward.hit && hitForward.distance < raycastDistance;
        // if (hitObstacle)
        // {   
        //     Vector3 newDirection; 
        //     // if object is on the right move left
        //     if (hitForward.position.x > transform.position.x)
        //     {
        //         newDirection =  Vector3.right;
        //         // _agent.Move(-newDirection * Time.deltaTime); 
        //     }

        //     // if object is on the left move right
        //     else
        //     {
        //         newDirection = Vector3.left;
        //     }

        //     _agent.Move(newDirection * movingSmoothness * Time.deltaTime); 

        //     Debug.Log("Hit something in front of me");
        // }

    }

    /// <summary> Add obstacle component to the object. </summary>
    private static void addObstaleTo(GameObject obj)
    {
        obj.AddComponent<NavMeshObstacle>();
    
    }

    public bool IsOutsideNavMesh(Vector3 objPosition)
    {
        NavMeshHit hit;
        float onMeshThreshold = 3; // adjust this value based on your needs

        if (NavMesh.SamplePosition(objPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check vertical alignment
            if (Mathf.Approximately(objPosition.x, hit.position.x) && Mathf.Approximately(objPosition.z, hit.position.z))
            {
                // Check if object is above the navmesh
                return objPosition.y > hit.position.y;
            }
        }

        return true; // object is outside the navmesh
    }
}