using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer audioMixer;  // Reference to the audio mixer to adjust volume levels
    
    public static MusicManager Instance { get; private set; }  // Singleton instance of the MusicManager

    private void Awake()
    {   
        // Singleton pattern to ensure only one instance of MusicManager exists
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Set the volume of the music
    public void SetMusicVolume(float value)
    {
        // Dividing by 3 to match the expected volume range
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20); // Convert linear value to logarithmic decibels
    }

    // Set the volume of the sound effects
    public void SetSFXVolume(float value)
    {   
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    // Set the volume of the master audio
    public void SetMasterVolume(float value)
    {   
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    // GETTERS 

    // Get the volume of the music of the parameter
    public float GetVolume(string parameter)
    {
        float value;
        audioMixer.GetFloat(parameter, out value);
        // Convert logarithmic decibels to linear value
        return Mathf.Pow(10, value / 20);
    }
}