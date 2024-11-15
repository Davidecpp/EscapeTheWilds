using UnityEngine;

public class PlatformController : MonoBehaviour
{
    // The force applied to the player when they land on the platform
    public float jumpForce = 10f;
    
    // When the player collides with the platform, apply a bouncing force
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with the platform is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody component of the player
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            // If the player has a Rigidbody, apply the bounce force
            if (rb != null)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply upward force for bounce
            }
        }
    }
}