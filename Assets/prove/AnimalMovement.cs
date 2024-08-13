using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public float moveSmoothTime;
    public float gravityStrenght;
    public float jumpStrenght;
    public float walkSpeed;
    public float runSpeed;
    public float boostSpeed;
    private float boostDuration;

    private CharacterController _controller;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;
    private Vector3 currentForceVelocity;
    private float currentBoostTime;
    
    private ProvaCamera cameraController;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        cameraController = GetComponentInChildren<ProvaCamera>();
    }

    void Update()
    {
        Vector3 playerInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };
        if (playerInput.magnitude > 1f)
        {
            playerInput.Normalize();
        }

        Vector3 moveVector = transform.TransformDirection(playerInput);
        float currentSpeed = cameraController.isSprinting ? runSpeed : walkSpeed;
        
        if (currentBoostTime > 0)
        {
            currentSpeed = boostSpeed;
            currentBoostTime -= Time.deltaTime;
        }

        currentMoveVelocity = Vector3.SmoothDamp(
            currentMoveVelocity, moveVector * currentSpeed, ref moveDampVelocity, moveSmoothTime);

        _controller.Move(currentMoveVelocity * Time.deltaTime);

        Ray groundCheck = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheck, 0.5f))
        {
            currentForceVelocity.y = -2f;

            if (Input.GetKey(KeyCode.Space))
            {
                currentForceVelocity.y = jumpStrenght;
            }
        }
        else
        {
            currentForceVelocity.y -= gravityStrenght * Time.deltaTime;
        }

        _controller.Move(currentForceVelocity * Time.deltaTime);
    }

    public void ActivateBoost(float duration)
    {
        currentBoostTime = duration;
    }
}