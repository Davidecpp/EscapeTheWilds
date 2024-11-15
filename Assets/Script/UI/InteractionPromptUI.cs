using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private UnityEngine.Camera mainCamera;  // Camera reference to align UI with the player's view
    [SerializeField] private GameObject uiPanel;  // The UI panel showing the interaction prompt
    [SerializeField] private TextMeshProUGUI promptText;  // Text component for displaying the prompt

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = UnityEngine.Camera.main;  // Get the main camera
        uiPanel.SetActive(false);  // Initially hide the interaction prompt UI panel
    }

    // LateUpdate ensures UI updates after the camera has moved (for proper alignment)
    private void LateUpdate()
    {
        var rotation = mainCamera.transform.rotation;  // Get the camera's current rotation
        // Make the UI panel face the camera by adjusting its rotation
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool isDisplayed = false;  // Flag to check if the prompt is currently being displayed

    // Set up the interaction prompt UI
    public void SetUp(string prompt)
    {
        promptText.text = prompt;  // Set the text for the prompt
        uiPanel.SetActive(true);   // Show the UI panel
        isDisplayed = true;        // Mark the prompt as displayed
    }

    // Close the interaction prompt UI
    public void Close()
    {
        uiPanel.SetActive(false);  // Hide the UI panel
        isDisplayed = false;       // Mark the prompt as not displayed
    }
}