using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProvaCamera : MonoBehaviour
{
    public Transform playerCamera;
    public Vector2 sensitivities;
    public Transform animal;
    
    // Sprint
    public Slider sprintSlider;
    public float normalDistance = 5f;
    public float sprintDistance = 3f;
    public float transitionSpeed = 5f;
    public float sprintDuration = 5f;
    public float sprintRechargeSpeed = 0.5f;
    public bool isSprinting = false;
    private float sprintTimer;
    
    // Camera shake
    public float shakeDuration = 0.1f;  
    public float shakeMagnitude = 0.1f;
    public float shakeDelay = 0.2f;
    private Vector3 initialCameraPosition;

    public bool human = false;

    private Vector2 _rotation;
    
    private CharacterController controller;
    private MenuManager menuManager;

    void Start()
    {
        sprintTimer = sprintDuration;
        initialCameraPosition = playerCamera.localPosition;
        controller = animal.GetComponent<CharacterController>();
        menuManager = FindObjectOfType<MenuManager>();
    }

    void Update()
    {
        HandleCameraRotation();
        HandleSprint();
        UpdateCameraPosition();

        if (!menuManager.menu.activeSelf && !human)
        {
            // Shakes camera when left mouse button pressed
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(CameraShake());
            }
        }
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
    
    // Sprint
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

    // Makes the camera shake
    private IEnumerator CameraShake()
    {
        yield return new WaitForSeconds(shakeDelay); 
        
        Vector3 originalPosition = playerCamera.localPosition; 
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Random movement around the original position
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            playerCamera.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null; 
        }
        playerCamera.localPosition = originalPosition;
    }
}
