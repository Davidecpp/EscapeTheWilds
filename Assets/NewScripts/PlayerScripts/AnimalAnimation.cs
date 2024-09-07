using System;
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

    /*void Update()
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
                _animator.SetBool("IsIdling", false);
            }
            // Camminata
            else if (isWalking)
            {
                _animator.SetBool("IsWalking", true);
                _animator.SetBool("IsRunning", false);
                _animator.SetBool("IsIdling", false);
            }
            else
            {
                _animator.SetBool("IsWalking", false);
                _animator.SetBool("IsRunning", false);
                _animator.SetBool("IsIdling", true);
            }

        }
    }*/

    private void Update()
    {
        if (_animator != null)
        {
            bool isWalking = Input.GetKey(KeyCode.W);
            bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space);

            if (!isJumping)
            {
                _animator.SetBool("IsIdling", true);
                _animator.SetBool("IsWalking", false);

                if (isWalking && !isJumping)
                {
                    //Debug.Log("IsWalking");
                    _animator.SetBool("IsWalking", true);
                    _animator.SetBool("IsIdling", false);
                
                }
                if (jumpKeyPressed)
                {
                    _animator.SetBool("IsIdling", false);
                    StartCoroutine(JumpAnimation(1f));
                }
            }
            
            

            /*if (isWalking && jumpKeyPressed) //solo questo funziona per saltare camminando
            {
                _animator.SetTrigger("TrJump");
            }*/
            
            
            /*if (jumpKeyPressed)
            {
                _animator.SetBool("IsWalking", true);
                _animator.SetTrigger("TrJump");
            }
            if (isWalking)
            {
                _animator.SetBool("IsIdling", false);
                _animator.SetBool("IsWalking", true);
            }*/
            
        }
    }

    private IEnumerator JumpAnimation(float seconds)
    {
        isJumping = true;
        Debug.Log("IS jumping");
        _animator.SetBool("IsWalking", false);
        _animator.SetTrigger("TrJump");
        yield return new WaitForSeconds(seconds);
        _animator.ResetTrigger("TrJump");
        _animator.SetBool("IsWalking", true);
        Debug.Log("jump finished");
        isJumping = false;
    }

    // Metodo per gestire la fine del salto (chiama questo metodo dall'animazione)
    public void OnJumpComplete()
    {
        isJumping = false;
    }
}