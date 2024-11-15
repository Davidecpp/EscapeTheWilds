using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    public Transform playerCamera; // Reference to the player's camera
    public Vector2 sensitivities; // Sensitivity for mouse input (X and Y axis)
    public Transform animal; // Reference to the player's animal (the character)

    // Sprint variables
    public float normalDistance = 5f; // Normal distance of the camera from the player
    public float sprintDistance = 3f; // Distance of the camera when sprinting
    public float transitionSpeed = 5f; // Speed of camera transition when changing distances
    public float sprintDuration; // Duration for how long the player can sprint
    public float sprintRechargeSpeed = 0.5f; // Speed at which sprint duration recharges
    public bool isSprinting = false; // Boolean to check if the player is currently sprinting
    private float sprintTimer; // Timer to track the remaining sprint duration

    // Camera shake variables
    public float shakeDuration = 0.1f; // Duration of the camera shake
    public float shakeMagnitude = 0.1f; // Magnitude (intensity) of the shake
    public float shakeDelay = 0.2f; // Delay before the shake starts
    private Vector3 initialCameraPosition; // The initial position of the camera for reference

    public bool human = false; // A boolean to track if the character is human

    private Vector2 _rotation; // Rotation values for the camera

    private CharacterController controller; // Reference to the CharacterController for the player
    private MenuManager menuManager; // Reference to the MenuManager (for menu control)
    private CanvasManager _canvasManager; // Reference to the CanvasManager (for UI control)

    private void Awake()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeReferences()
    {
        // Initialize player camera reference
        if (playerCamera == null)
        {
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
            if (playerCamera != null)
            {
                initialCameraPosition = playerCamera.localPosition;
            }
        }

        // Initialize animal (player) reference
        if (animal == null)
        {
            animal = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (animal != null)
            {
                controller = animal.GetComponent<CharacterController>();
            }
        }

        // Initialize other necessary references
        menuManager = FindObjectOfType<MenuManager>();
        _canvasManager = FindObjectOfType<CanvasManager>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-initialize references when a new scene is loaded
        InitializeReferences();
    }

    void Start()
    {
        // Set sprint duration to player's stamina
        var player = FindObjectOfType<PlayerStats>();
        sprintDuration = player.GetStamina();
        
        // Initialize sprint timer and camera position
        sprintTimer = sprintDuration;
        initialCameraPosition = playerCamera.localPosition;
        
        controller = animal.GetComponent<CharacterController>(); // Get Controller component
        menuManager = FindObjectOfType<MenuManager>();
        _canvasManager = FindObjectOfType<CanvasManager>();
    }

    void Update()
    {
        // Early return if camera or animal references are not set
        if (playerCamera == null || animal == null)
        {
            Debug.LogWarning("playerCamera or animal not assigned. Check if they exist in the scene.");
            return;
        }

        // Handle camera rotation (mouse input)
        HandleCameraRotation();

        // Handle sprint mechanics (sprinting, duration, and recharge)
        HandleSprint();

        // Update the camera position based on sprinting or normal movement
        UpdateCameraPosition();

        // If menu is not active and the character is not human, perform camera shake on mouse click
        if (!menuManager.menu.activeSelf && !human)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button click
            {
                StartCoroutine(CameraShake());
            }
        }
    }

    private void HandleCameraRotation()
    {
        // Get mouse input for camera rotation
        Vector2 mouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        // Update rotation based on mouse input
        _rotation.x -= mouseInput.y * sensitivities.y;
        _rotation.y += mouseInput.x * sensitivities.x;

        // Clamp the X-axis rotation to prevent the camera from flipping upside down
        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        // Apply the rotations to the camera
        transform.eulerAngles = new Vector3(0f, _rotation.y, 0f);
        playerCamera.localEulerAngles = new Vector3(_rotation.x, 0f, 0f);
    }
    
    // Handle sprinting mechanics (starting, stopping, timer, recharge)
    private void HandleSprint()
    {
        // Start sprinting when LeftShift is pressed and there is sprint time remaining
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprintTimer > 0)
        {
            isSprinting = true;
        }
        // Stop sprinting when LeftShift is released or the sprint timer is 0
        else if (Input.GetKeyUp(KeyCode.LeftShift) || sprintTimer <= 0)
        {
            isSprinting = false;
        }

        // Decrease the sprint timer if sprinting
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
            // Recharge the sprint timer if not sprinting
            if (sprintTimer < sprintDuration)
            {
                sprintTimer += sprintRechargeSpeed * Time.deltaTime;
                sprintTimer = Mathf.Clamp(sprintTimer, 0, sprintDuration);
            }
        }

        // Update sprint slider UI
        _canvasManager.sprintSlider.value = sprintTimer / sprintDuration;
    }

    private void UpdateCameraPosition()
    {
        // Set the target camera distance based on whether the player is sprinting
        float targetDistance = isSprinting ? sprintDistance : normalDistance;
        // Calculate the new camera position based on the player's position
        Vector3 targetPosition = animal.position - animal.forward * targetDistance;
        targetPosition.y = animal.position.y + initialCameraPosition.y;
        
        // Smoothly transition the camera position
        playerCamera.position = Vector3.Lerp(playerCamera.position, targetPosition, transitionSpeed * Time.deltaTime);
    }

    // Camera shake effect coroutine
    private IEnumerator CameraShake()
    {
        yield return new WaitForSeconds(shakeDelay); // Delay before starting the shake
        
        Vector3 originalPosition = playerCamera.localPosition; // Store the initial camera position
        float elapsed = 0.0f;

        // Shake the camera until the specified duration has passed
        while (elapsed < shakeDuration)
        {
            // Apply random movement to simulate shaking
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            playerCamera.localPosition = randomPoint;

            elapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Return the camera to its original position after the shake
        playerCamera.localPosition = originalPosition;
    }
}
