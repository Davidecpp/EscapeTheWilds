using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    // UI elements for dialogue display
    public TextMeshProUGUI textComponent;  // The TextMeshPro component displaying dialogue text
    public float textSpeed;               // Speed at which the text is typed out
    public Image box;                     // Background box for the dialogue

    public GameObject skip;               // UI element to show/hide skip option (e.g., button)

    private string[] lines;               // Array holding all dialogue lines
    private int index;                    // Index to track the current dialogue line
    public bool isActive;                 // Flag to check if dialogue is active or not

    void Start()
    {
        textComponent.text = string.Empty; // Clear the dialogue text at the start
        isActive = false;                  // Set dialogue as inactive initially
    }

    void Update()
    {
        // Check for mouse click or enter key press to show the next dialogue line
        if (isActive && (Input.GetMouseButtonDown(0) || Keyboard.current.enterKey.wasPressedThisFrame))
        {
            // If the current line is fully displayed, go to the next line
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                // If the current line isn't fully displayed, skip typing and show full text
                StopAllCoroutines(); 
                textComponent.text = lines[index];
            }
        }
        HideBox();  // Update visibility of the dialogue box and skip button
    }

    // Hide the dialogue box and skip button when not active
    private void HideBox()
    {
        if (isActive)
        {
            // Ensure the box is visible and skip button is shown when dialogue is active
            SetImageAlpha(box, 1f);
            skip.SetActive(true);
        }
        else
        {
            // Make the box transparent and hide the skip button when dialogue is inactive
            SetImageAlpha(box, 0f);  
            skip.SetActive(false);
        }
    }

    // Change the alpha value of an image (used for fading the dialogue box)
    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;  // Get current color of the image
        color.a = alpha;            // Change the alpha value to make the image transparent or opaque
        image.color = color;        // Apply the new color to the image
    }

    // Set new dialogue lines to be displayed
    public void SetDialogue(string[] newLines)
    {
        // Prevent starting a new dialogue if one is already active
        if (isActive) return;

        lines = newLines;  // Set new dialogue lines
        index = 0;         // Start with the first line
        textComponent.text = string.Empty; // Clear any previous dialogue
        isActive = true;   // Set dialogue as active

        GameManager.Instance.PauseGame();  // Pause the game while the dialogue is active
        StartCoroutine(TypeLine());        // Start the coroutine to type the current line
    }

    // Coroutine to type out the dialogue line one character at a time
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())  // Iterate through each character in the current line
        {
            textComponent.text += c;  // Add each character to the text component
            yield return new WaitForSecondsRealtime(textSpeed); // Wait for the specified text speed
        }
    }

    // Move to the next line in the dialogue or end the dialogue if all lines are finished
    void NextLine()
    {
        if (index < lines.Length - 1)  // If there are more lines to display
        {
            index++;  // Move to the next line
            textComponent.text = string.Empty; // Clear the text
            StartCoroutine(TypeLine());        // Start typing the next line
        }
        else
        {
            EndDialogue();  // End the dialogue if there are no more lines
        }
    }

    // End the dialogue and resume the game
    void EndDialogue()
    {
        isActive = false;   // Set dialogue as inactive
        textComponent.text = string.Empty; // Clear the text
        GameManager.Instance.ResumeGame();  // Resume the game after dialogue ends
    }

    // Check and display initial dialogue based on the current scene
    public void CheckInitialDialogue()
    {
        if (GameManager.Instance.currentScene == 4)  // Example: If in scene 4, show this dialogue
        {
            SetDialogue(new[] { "Enter the house.", "Press WASD to move." });
        }
    }
}
