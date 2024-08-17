using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimalAnimation : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator != null)
        {
            if (Input.GetKey(KeyCode.Space)&& Input.GetKey(KeyCode.W))
            {
                _animator.SetTrigger("TrFly");
                Debug.Log("Jump");
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                _animator.SetTrigger("TrFly");
                Debug.Log("Jump");
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                _animator.SetTrigger("TrRun");
            }
            else if (Input.GetKey(KeyCode.W))
            {
                _animator.SetTrigger("TrWalk");
            }
            else
            {
                _animator.SetTrigger("TrIdle");
                
            }
        }
    }
    
}
