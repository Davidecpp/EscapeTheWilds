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
    [SerializeField] private Transform _target;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_target == null)
        {
            Debug.LogError("Target is not set");
        }
    }

    void Update()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
        }

    }
}