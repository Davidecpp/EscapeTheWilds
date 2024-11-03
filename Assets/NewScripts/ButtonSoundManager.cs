using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Find buttons in the scene
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            // Add event listener
            button.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}