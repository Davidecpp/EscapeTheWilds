using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{   
    // Dropdown for selecting resolution from the available options
    public TMP_Dropdown resolutionDropdown;
    
    // Array to store available screen resolutions
    Resolution[] resolutions;

    // Start is called before the first frame update
    private void Start()
    {   
        
        // Get all available screen resolutions
        resolutions = Screen.resolutions;

        // Clear any existing options in the dropdown
        resolutionDropdown.ClearOptions();

        // Variable to track the current resolution index (default to 0)
        int currentResolutionIndex = 0;

        // List to store string representations of the resolutions
        List<string> options = new List<string>();

        // Loop through all resolutions to populate the dropdown and find the current resolution
        for (int i = 0; i < resolutions.Length; i++)
        {
            // Format the resolution as "width x height"
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            // Check if the resolution matches the current screen resolution
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height )
            {
                // If matched, set the current index
                currentResolutionIndex = i;
            }
        }

        // Add the options to the dropdown
        resolutionDropdown.AddOptions(options);

        // Set the dropdown's current value to the current resolution
        resolutionDropdown.value = currentResolutionIndex;

        // Refresh the dropdown to reflect the current value
        resolutionDropdown.RefreshShownValue();
    }

    // Method to set the quality of the game (using quality settings defined in Unity)
    public void SetQuality(int quality)
    {
        // Set the quality level based on the input value
        QualitySettings.SetQualityLevel(quality);
    }

    // Method to toggle fullscreen mode
    public void SetFullscreen(bool isFullscreen)
    {
        // Set the screen mode to fullscreen or windowed based on the input
        Screen.fullScreen = isFullscreen;
    }

    // Method to set the screen resolution based on the dropdown selection
    public void SetResolution(int resolutionIndex)
    {
        // Get the selected resolution from the resolutions array
        Resolution resolution = resolutions[resolutionIndex];

        // Apply the selected resolution while keeping the current fullscreen setting
        UnityEngine.Device.Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Set the volume of the music
    public void SetMusicVolume(float value)
    {   
        MusicManager.Instance.SetMusicVolume(value); // Call the MusicManager instance to set the music volume
    }

    // Set the volume of the sound effects
    public void SetSFXVolume(float value)
    {   
        MusicManager.Instance.SetSFXVolume(value); // Call the MusicManager instance to set the SFX volume
    }
}
