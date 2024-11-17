using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// This class is responsible for the AI movement using unity NavMesh.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class RunAI : MonoBehaviour
{
    private NavMeshAgent _agent;  
    private int _walkableAreaMask;
    
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
        _agent.updatePosition = false;
        _walkableAreaMask = NavMesh.GetAreaFromName("Walkable");


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

        // if goal is not set, return
        if (!isGoalSet) return;

    

        // if AI is close to the goal, return
        if (_agent.remainingDistance < 0.5f) return;

        // if AI is outside the navmesh, return
        if (IsOnWalkable(transform.position)) return;

        // if AI is not moving, return
        if (_agent.velocity.magnitude == 0) return;
        

        // DRAW LINE TO NEXT POSITION
        Debug.DrawLine(transform.position, _agent.nextPosition, Color.magenta);

        // CHECK IF NEXT POSITION WILL COLLIDE WITH NOT WALKABLE AREA
        Vector3 nextPosition = transform.position + _agent.velocity.normalized * 0.5f;
        if (!IsOnWalkable(nextPosition))
        {
            Debug.Log("Next position is not walkable");
            _agent.velocity = Vector3.zero;
            return;
        }

        // AVOID MOVVING ON BORDERS
        // NavMeshHit hitForward;
        // _agent.Raycast(Vector3.forward, out hitForward);
        

        // if hit something in near front and is outside the navmesh
        // bool hitObstacle = hitForward.hit && hitForward.distance < raycastDistance;
        // if (hitObstacle && IsOutsideNavMesh(hitForward.position)) 
        // {
        //     //addObstaleTo(hitForward.collider.gameObject);
        // }
        
        


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

    public bool IsOnWalkable(Vector3 objPosition, float tollerance = 0.1f)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(objPosition, out hit, tollerance, _walkableAreaMask);
        return hit.hit;
    }
}