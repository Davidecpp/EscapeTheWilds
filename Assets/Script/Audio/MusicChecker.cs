using System.Collections.Generic;
using UnityEngine;

public class MusicChecker : MonoBehaviour
{   
    public AudioSource toMute;  // Reference to the audio source to pause when another music source is playing

    private List<AudioSource> audioSources;  // Array to store all audio sources in the scene


    // Update is called once per frame
    void Update()
    {   
        if (isActiveAndEnabled)
        {
            if (toMute == null)
            {
                // If no audio source is assigned, try to get from component
                toMute = GetComponent<AudioSource>();
            }
            // Find other audio sources in the scene
            audioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
            // remove toMute from the list
            audioSources.Remove(toMute);


            if (! toMute.mute){
                // If the audio source is not muted, check if other music is playing
                if (OtherMusicPlaying())
                {
                    // Mute the audio source if other music is playing
                    toMute.mute = true;
                }
            }

            else
            {
                // If the audio source is muted, check if other music is playing
                if (! OtherMusicPlaying())
                {
                    // Unmute the audio source if no other music is playing
                    toMute.mute = false;
                }
            }
        
        }


    }

    private bool OtherMusicPlaying()
    {   
        if (audioSources == null || audioSources.Count == 0)
        {
            // If no audio source is found, return false
            return false;
        }

        // Check if any audio source is playing to 'Music' to prevent multiple music tracks from playing
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource!= toMute && audioSource.isPlaying && audioSource.outputAudioMixerGroup.name == "Music")
            {
                return true;
            }
        }
        return false;
    }
}
