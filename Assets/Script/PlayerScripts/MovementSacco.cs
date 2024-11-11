using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Namespace for all movement scripts.
/// </summary>
namespace Movements
{   
    /// <summary>
    /// namespace for all movement scripts that use a Rigidbody.
    /// </summary>
    namespace RigidbodyMov
    {
        public abstract class RigidbodyBase : MonoBehaviour
        {
            [SerializeField] protected Rigidbody _rigidbody;
        }  


    //         [AddComponentMenu("PlayerScripts/RigidBodyMovement")]
    //     /// <summary>
    //     /// This class handles the scripted movement of the player using a CharacterController.
    //     /// It is used to move the player in a specific way, for example in a cutscene.
    //     /// </summary>
    //     public class RigidBodyMovement : MonoBehaviour
    //     {

    //         private Rigidbody _rigidbody;

    //         void Start()
    //         {
    //             _rigidbody = GetComponent<Rigidbody>();
                
    //         }

    // }        
    // }
    }


    
}

