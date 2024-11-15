using UnityEngine;
using UnityEngine.UI;

public class GIFAnimator : MonoBehaviour
{
    // RawImage component where the GIF will be displayed
    public RawImage rawImage;
    
    // Array to hold all the frames of the GIF (Textures)
    public Texture[] frames;
    
    // The rate at which frames should change (time in seconds per frame)
    public float frameRate = 0.1f;

    // Keeps track of the current frame being displayed
    private int currentFrame;
    
    // Timer to control frame switching based on frameRate
    private float timer;

    // Update is called once per frame
    void Update()
    {
        // If there are no frames, return and do nothing
        if (frames.Length == 0) return;

        // Increment the timer by the time that has passed since the last frame update
        timer += Time.deltaTime;

        // If enough time has passed (based on the frameRate), update the frame
        if (timer >= frameRate && rawImage != null)
        {
            // Move to the next frame. Use modulo to loop back to the first frame when reaching the end
            currentFrame = (currentFrame + 1) % frames.Length;
            
            // Set the RawImage's texture to the current frame
            rawImage.texture = frames[currentFrame];
            
            // Reset the timer to start counting for the next frame update
            timer = 0f;
        }
    }
}