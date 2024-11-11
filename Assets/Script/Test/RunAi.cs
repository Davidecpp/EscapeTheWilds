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

    }

    void Update()
    {

    }
}