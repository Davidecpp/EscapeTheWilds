using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimalAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool isJumping = false;
    public bool isAttacking = false;
    private CharacterController _characterController;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_animator != null)
        {
            bool isWalking = Input.GetKey(KeyCode.W);
            bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space);
            bool attackPressed  = Input.GetMouseButtonDown(0);

            if (!isJumping && !isAttacking)
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
                    StartCoroutine(JumpAnimation(0.5f));
                }
                if (attackPressed)
                {
                    _animator.SetBool("IsIdling", false);
                    StartCoroutine(AttackAnimation(0.5f,5f));
                }
            }

            
        }
        
    }

    private IEnumerator JumpAnimation(float seconds)
    {
        isJumping = true;
        _animator.SetBool("IsWalking", false);
        _animator.SetTrigger("TrJump");
        yield return new WaitForSeconds(seconds);
        _animator.ResetTrigger("TrJump");
        _animator.SetBool("IsWalking", true);
        isJumping = false;
    }
    private IEnumerator AttackAnimation(float seconds, float moveSpeed)
    {
        isAttacking = true;
        _animator.SetBool("IsWalking", false);
        _animator.SetTrigger("TrAttack");

        float elapsedTime = 0f;

        // Il personaggio si muoverà durante l'attacco
        while (elapsedTime < seconds)
        {
            // Muove il player in avanti mentre attacca
            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _animator.ResetTrigger("TrAttack");
        _animator.SetBool("IsWalking", true);
        //Debug.Log("attack finished");

        isAttacking = false;
    }

}