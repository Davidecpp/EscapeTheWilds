using System.Collections;
using UnityEngine;

public class AnimalAnimation : MonoBehaviour
{
    private Animator _animator;                   // Reference to the Animator component for controlling animations
    private CharacterController _characterController;  // Reference to the CharacterController component for movement
    
    // Conditions to manage animation states
    private bool isJumping = false;              // Boolean to track if the animal is jumping
    public bool isAttacking = false;             // Boolean to track if the animal is attacking

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();    // Get the Animator component attached to the same GameObject
        _characterController = GetComponent<CharacterController>();  // Get the CharacterController component attached to the GameObject
    }

    // Update is called once per frame
    private void Update()
    {
        if (_animator != null)  // Ensure the animator is not null before trying to control it
        {
            bool isWalking = Input.GetKey(KeyCode.W);          // Check if the "W" key is pressed (moving forward)
            bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space);  // Check if the space key is pressed (jump)
            bool attackPressed = Input.GetMouseButtonDown(0);  // Check if the left mouse button is clicked (attack)

            if (!isJumping && !isAttacking)  // Only perform actions if the animal is not already jumping or attacking
            {
                _animator.SetBool("IsIdling", true);  // Set the "Idling" animation state to true
                _animator.SetBool("IsWalking", false);  // Set the "Walking" animation state to false

                if (isWalking && !isJumping)  // If the animal is walking and not jumping
                {
                    _animator.SetBool("IsWalking", true);  // Set the "Walking" animation state to true
                    _animator.SetBool("IsIdling", false);  // Set the "Idling" animation state to false
                }

                if (jumpKeyPressed)  // If the jump key is pressed
                {
                    _animator.SetBool("IsIdling", false);  // Set "Idling" to false when jumping
                    StartCoroutine(JumpAnimation(0.5f));  // Start the jump animation with a delay of 0.5 seconds
                }

                if (attackPressed)  // If the attack key (mouse button) is pressed
                {
                    _animator.SetBool("IsIdling", false);  // Set "Idling" to false when attacking
                    StartCoroutine(AttackAnimation(0.5f, 5f));  // Start the attack animation with a delay of 0.5 seconds and a movement speed of 5 units
                }
            }
        }
    }

    // Jump animation coroutine
    private IEnumerator JumpAnimation(float seconds)
    {
        isJumping = true;  // Set jumping state to true
        
        _animator.SetBool("IsWalking", false);  // Disable walking animation during jump
        _animator.SetTrigger("TrJump");  // Trigger the jump animation

        yield return new WaitForSeconds(seconds);  // Wait for the specified duration (e.g., how long the jump lasts)

        _animator.ResetTrigger("TrJump");  // Reset the jump trigger after the animation completes
        _animator.SetBool("IsWalking", true);  // Enable walking animation after the jump
        
        isJumping = false;  // Set jumping state to false
    }

    // Attack animation coroutine
    private IEnumerator AttackAnimation(float seconds, float moveSpeed)
    {
        isAttacking = true;  // Set attacking state to true
        _animator.SetBool("IsWalking", false);  // Disable walking animation during attack
        _animator.SetTrigger("TrAttack");  // Trigger the attack animation

        float elapsedTime = 0f;  // Track the elapsed time of the attack

        // Move the character forward while performing the attack animation
        while (elapsedTime < seconds)
        {
            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));  // Move the character forward based on the specified move speed
            elapsedTime += Time.deltaTime;  // Update elapsed time
            yield return null;  // Wait until the next frame
        }

        _animator.ResetTrigger("TrAttack");  // Reset the attack trigger after the animation completes
        _animator.SetBool("IsWalking", true);  // Enable walking animation after the attack
        isAttacking = false;  // Set attacking state to false
    }
}
