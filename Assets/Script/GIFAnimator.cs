using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GIFAnimator : MonoBehaviour
{
    public RawImage rawImage;
    public Texture[] frames; 
    public float frameRate = 0.1f; 

    private int currentFrame;
    private float timer;

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate && rawImage != null)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            rawImage.texture = frames[currentFrame];
            timer = 0f;
        }
    }
}