using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;  // Audio clip to play when a button is clicked
    private AudioSource audioSource;  // AudioSource component to play sounds

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Find all Button components in the scene
        Button[] buttons = FindObjectsOfType<Button>();
        
        // Loop through each button and add a listener to play the click sound when clicked
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayClickSound);  // Add event listener for button click
        }
    }

    // Play the click sound when a button is clicked
    void PlayClickSound()
    {
        // Check if the click sound is assigned before playing it
        if (clickSound != null)
        {
            // Play the click sound as a one-shot audio clip
            audioSource.PlayOneShot(clickSound);
        }
    }
}