using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    // Makes music persisten from scene to scene
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    public void SetVolume(float value)
    {
        audioMixer.SetFloat("musicvolume", value / 3);
    }
}