using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    // Dash
    public float dashSpeed = 20f; 
    public float dashDuration = 0.5f; 
    private bool isDashing = false;
    public TrailRenderer trailRenderer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isDashing)
        {
            StartCoroutine(VerticalDash());
        }
    }
    
    IEnumerator VerticalDash()
    {
        isDashing = true;
        float dashTime = 0f;
        dashSpeed = 1000;
        dashDuration = 0.05f;
        
        trailRenderer.emitting = true;
        
        while (dashTime < dashDuration)
        {
            dashTime += Time.deltaTime;

            // Raycast to find obstacles
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, dashSpeed * Time.deltaTime))
            {
                // Interrupt the dash if there is an obstacle
                if (hit.collider != null)
                {
                    break; 
                }
            }
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

            yield return null;
        }
        trailRenderer.emitting = false;
        isDashing = false;
    }
}



