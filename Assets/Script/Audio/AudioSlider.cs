using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{   
    [Tooltip("The audio parameter to adjust (MusicVolume, SFXVolume, MasterVolume)")]
    public string parameter;

    // Start is called before the first frame update
    void Start()
    {
        Slider slider = GetComponent<Slider>();

        // Set the slider value to the current volume
        slider.value = MusicManager.Instance.GetVolume(parameter);
    }
}
