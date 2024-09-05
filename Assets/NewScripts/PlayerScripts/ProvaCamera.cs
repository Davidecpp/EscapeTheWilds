using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProvaCamera : MonoBehaviour
{
    public Transform playerCamera;
    public Vector2 sensitivities;
    public Transform animal;
    public Slider sprintSlider;
    public float normalDistance = 5f;
    public float sprintDistance = 3f;
    public float transitionSpeed = 5f;
    public float sprintDuration = 5f;
    public float sprintRechargeSpeed = 0.5f;

    private Vector2 _rotation;
    public bool isSprinting = false;
    private float sprintTimer;
    private Vector3 initialCameraPosition;
    private CharacterController controller;

    void Start()
    {
        sprintTimer = sprintDuration;
        initialCameraPosition = playerCamera.localPosition;
        controller = animal.GetComponent<CharacterController>(); 
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleCameraRotation();
        HandleSprint();
        UpdateCameraPosition();
    }

    private void HandleCameraRotation()
    {
        Vector2 mouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        _rotation.x -= mouseInput.y * sensitivities.y;
        _rotation.y += mouseInput.x * sensitivities.x;

        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, _rotation.y, 0f);
        playerCamera.localEulerAngles = new Vector3(_rotation.x, 0f, 0f);
    }

    private void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprintTimer > 0)
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || sprintTimer <= 0)
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0)
            {
                sprintTimer = 0;
                isSprinting = false;
            }
        }
        else
        {
            if (sprintTimer < sprintDuration)
            {
                sprintTimer += sprintRechargeSpeed * Time.deltaTime;
                sprintTimer = Mathf.Clamp(sprintTimer, 0, sprintDuration);
            }
        }

        sprintSlider.value = sprintTimer / sprintDuration;
    }

    private void UpdateCameraPosition()
    {
        float targetDistance = isSprinting ? sprintDistance : normalDistance;
        Vector3 targetPosition = animal.position - animal.forward * targetDistance;
        targetPosition.y = animal.position.y + initialCameraPosition.y;
        playerCamera.position = Vector3.Lerp(playerCamera.position, targetPosition, transitionSpeed * Time.deltaTime);
    }
}