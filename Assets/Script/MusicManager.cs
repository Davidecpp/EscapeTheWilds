using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer audioMixer;  // Reference to the audio mixer to adjust volume levels
    
    // Set the volume of the music
    public void SetVolume(float value)
    {
        // Adjust the music volume by setting a "musicvolume" parameter in the audio mixer
        audioMixer.SetFloat("musicvolume", value / 3); // Dividing value by 3 to scale the volume properly
    }
}