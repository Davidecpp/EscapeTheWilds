using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalCameraController : MonoBehaviour
{
    public Transform animal; 
    public Transform playerCamera; 
    public Slider sprintSlider; 
    public float normalDistance = 5f; // Distanza normale della camera dall'animale
    public float sprintDistance = 3f; // Distanza della camera dall'animale durante lo sprint
    public float transitionSpeed = 5f; // Velocità di transizione della camera
    public float sprintDuration = 5f; // Durata dello sprint in secondi
    public float sprintRechargeSpeed = 0.5f; // Velocità di ricarica dello sprint al secondo

    private bool isSprinting = false;
    private float sprintTimer = 0f;
    
    private Vector3 initialCameraPosition;

    void Start()
    {
        // Memorizza la posizione iniziale della camera
        initialCameraPosition = playerCamera.localPosition;
        sprintTimer = sprintDuration;
    }

    void Update()
    {
        //sprint
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprintTimer > 0)
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        //reduce timer if sprinting
        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0)
            {
                sprintTimer = 0;
                isSprinting = false;
            }
        }
        //recharge
        else
        {
            if (sprintTimer < sprintDuration)
            {
                sprintTimer += sprintRechargeSpeed * Time.deltaTime;
                sprintTimer = Mathf.Clamp(sprintTimer, 0, sprintDuration);
            }
        }
        
        sprintSlider.value = sprintTimer / sprintDuration;

        // Calcola la distanza target in base allo stato dello sprint
        float targetDistance = isSprinting ? sprintDistance : normalDistance;

        // Calcola la posizione target della camera mantenendo la coordinata Y della posizione iniziale della camera
        Vector3 targetPosition = animal.position - animal.forward * targetDistance;
        targetPosition.y = animal.position.y + initialCameraPosition.y; // Aggiorna la coordinata Y in base alla posizione Y dell'animale
        playerCamera.position = Vector3.Lerp(playerCamera.position, targetPosition, transitionSpeed * Time.deltaTime);
    }
}
