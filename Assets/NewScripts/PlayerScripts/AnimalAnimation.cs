using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimalAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool isJumping = false;
    private CharacterController _characterController;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_animator != null)
        {
            bool isWalking = Input.GetKey(KeyCode.W);
            bool isRunning = isWalking && Input.GetKey(KeyCode.LeftShift);
            bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space);

            // Salto
            if (jumpKeyPressed && !isJumping)
            {
                _animator.SetTrigger("TrJump");
                isJumping = true;
                Debug.Log("Jump");
            }

            // Corsa
            if (isRunning)
            {
                _animator.SetBool("IsRunning", true);
                _animator.SetBool("IsWalking", false);
            }
            // Camminata
            else if (isWalking)
            {
                _animator.SetBool("IsWalking", true);
                _animator.SetBool("IsRunning", false);
            }
            else
            {
                _animator.SetBool("IsWalking", false);
                _animator.SetBool("IsRunning", false);
                _animator.SetBool("IsIdling", true);
            }

        }
    }

    // Metodo per gestire la fine del salto (chiama questo metodo dall'animazione)
    public void OnJumpComplete()
    {
        isJumping = false;
    }
}