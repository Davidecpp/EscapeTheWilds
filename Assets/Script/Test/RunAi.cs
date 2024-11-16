using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// This class is responsible for the AI movement using unity NavMesh.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class RunAI : MonoBehaviour
{
    private NavMeshAgent _agent;  
    
    [Tooltip("The target that the AI will move towards")]
    [SerializeField] private Transform _goal;

    [Tooltip("The 'outside tolerance' for checking if the goal is outside the 'Walkable' mesh. The higher the value, the more the goal can be outside the mesh.")]
    public float goalCheckTollerance = 1f; 


    [Tooltip("If true, the AI will search an Object in the scene with tag 'Goal'.")]
    public bool searchForGoal = false;

    [HideInInspector]
    public bool isGoalSet = false;


    /// <summary> int value of the walkable area for 'mask'. </summary>
    private int _walkableArea;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = true;

        _walkableArea = 1 << NavMesh.GetAreaFromName("Walkable");

        // Set auto-braking and auto-repath to true
        _agent.autoBraking = true;
        _agent.autoRepath = true;
        _agent.areaMask = _walkableArea;


        // GOAL SEARCH
        if (searchForGoal && _goal == null)
        {
            // try get _target from tag in the scene
            GameObject targetObject = GameObject.FindWithTag("Goal");
            _goal = targetObject.transform;
        }

        // SET DESTINATION
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


    [Tooltip("Max distance of the ray cast in front of the AI.")]
    public float maxDistance = 10f;

    [Tooltip("The point where the raycast starts.")]
    public Transform raycastStartPoint;

    [Tooltip("The radius of the sphere cast in front of the AI.")]
    public float viewRadius = 10f;

    void Update()
    {   

        if (!isGoalSet)
        {   
            Debug.LogError("Goal is not set");
            return;
        }

        if (IsOutsideWalkable(_goal.position, goalCheckTollerance))
        {
            Debug.LogError("Goal is outside Walkable");
            return;
        }


        // RaycastHit hitForward;
        // Physics.SphereCast(raycastStartPoint.position, 0.5f, raycastStartPoint.forward, out hitForward, maxDistance, _walkableArea);
        // Physics.SphereCast(raycastStartPoint.position, viewRadius, raycastStartPoint.forward, out hitForward, maxDistance);
        
        // AVOID MOVING ON BORDERS
  
        // if hit something in near front of the AI
        // bool hitObstacle = hitForward.point != Vector3.zero;

        NavMeshHit hitForward;
        _agent.Raycast(raycastStartPoint.forward, out hitForward);

        bool hitObstacle = hitForward.hit && hitForward.distance < maxDistance;
        // if (hitObstacle &&  IsOutsideWalkable(hitForward.point, 1f))
        if (hitObstacle &&  IsOutsideWalkable(hitForward.position, 1f))
        {   
            Debug.Log("Obstacle is outside Walkable"+ hitForward.position);
            // Change direction of the agent
            ChangeDirectionRelTo(hitForward.position);
        }

    }

    private void OnDrawGizmos()
    {   // Color red and transparent
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        // Draw raycast
        Gizmos.DrawRay(raycastStartPoint.position, raycastStartPoint.forward * maxDistance);
    }

//--STATIC METHODS---------------------------------------------------------------------------------------------------------------------------- 
    
   
    /// <summary>
    /// Check if a position is outside the 'Walkable' mesh .
    /// </summary>
    /// <param name="objPosition">The position to check.</param>
    /// <param name="checkTollerance">The 'outside tollerance' for checking if a position is outside the 'Walkable' mesh.
    /// The higher the value, the more the parameter passed to 'IsOutsideWalkable' can be outside the mesh.</param>
    /// <returns>True if the position is outside the 'Walkable' mesh, false otherwise.</returns>
    public bool IsOutsideWalkable(Vector3 objPosition, float checkTollerance = 1f)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(objPosition, out hit, checkTollerance, _walkableArea);
        return !hit.hit;
    }


  

//--PRIVATE METHODS---------------------------------------------------------------------------------------------------------------------------- 
    
    [Tooltip("How much the AI will rotate when it raycasts objects in front.")]
    public float rotation =  0.5f; 
    [Tooltip("Smoothness of the AI rotation when it raycasts objects in front.")]
    public float rotSmoothness = 0.5f;
    
    
    /// <summary>
    /// Rotate the agent on opposite direction of relativeTo. 
    /// The rotation is determined by the 'rotation' field.
    /// The smoothness of the rotation is determined by the 'rotSmoothness' field.
    /// </summary>
    /// <param name="relativeTo"></param>
    private void ChangeDirectionRelTo(Vector3 relativeTo){
        // Rotate the agent on opposite direction of relativeTo
        Vector3 direction = (transform.position - relativeTo);
        Quaternion lookRotation = Quaternion.LookRotation(direction  *rotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSmoothness);
    }
}