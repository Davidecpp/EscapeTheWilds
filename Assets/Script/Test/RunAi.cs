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

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

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
            _agent.SetDestination(_goal.position);
        }

        
    }

    void Update()
    {
        // if (_target != null)
        // {
        //     _agent.SetDestination(_target.position);
        // }

    }

}