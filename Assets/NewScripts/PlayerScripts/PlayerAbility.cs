using System;
using System.Collections;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public string characterName;
    // Dash
    public float dashSpeed = 20f; 
    public float dashDuration = 0.5f; 
    private bool isDashing = false;
    public TrailRenderer trailRenderer;
    public GameObject dashSparkle;
    public AudioSource dashSound;
    
    // Venom cloud
    public GameObject venomSprayPrefab;
    public GameObject venomCloudPrefab;  
    public Transform venomSpawnPoint;   
    public float sprayCooldown = 5f;
    public float sprayDuration = 0.5f; 
    private bool canSpray = true;
    public AudioSource spraySound;
    
    // Mega Jump
    public float extraJump;
    public GameObject megaJumpParticles;
    
    public float abilityTime = 5.0f;
    public float abilityCooldown = 0;
    
    private CanvasManager _canvas;
    private Movement _movement;

    private void Start()
    {
        _canvas = FindObjectOfType<CanvasManager>();
        _movement = FindObjectOfType<Movement>();
        dashSound = GetComponent<AudioSource>();
        spraySound = GetComponent<AudioSource>();
        trailRenderer.emitting = false;
    }

    void Update()
    {
        HandleCooldown();

        if (Input.GetKeyDown(KeyCode.R) && abilityCooldown <= 0)
        {
            ActivateAbility();
        }

        if (_movement.megaJump)
        {
            InstantiateAndDestroy(megaJumpParticles, venomSpawnPoint.position, venomSpawnPoint.rotation, abilityTime);
        }
    }

    // Handles the ability cooldown
    private void HandleCooldown()
    {
        if (abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
            _canvas.isAbiliting = true;
        }
        else
        {
            _canvas.isAbiliting = false;
        }
    }

    // Activates player's ability based on characters name
    private void ActivateAbility()
    {
        switch (characterName)
        {
            case "Deer":
                if (!isDashing)
                    StartCoroutine(VerticalDash());
                break;
            case "Snake":
                if (canSpray)
                    StartCoroutine(SprayVenom());
                break;
            case "Rat":
                StartCoroutine(MegaJump());
                break;
        }
    }

    // Mega Jump
    IEnumerator MegaJump()
    {
        _movement.megaJump = true; 
        abilityCooldown = abilityTime; 
        yield return null; 
    }

    // Spray venom and generate a venom cloud
    IEnumerator SprayVenom()
    {
        canSpray = false;
        InstantiateAndDestroy(venomSprayPrefab, venomSpawnPoint.position, venomSpawnPoint.rotation, sprayDuration);
        spraySound.Play();
        
        Vector3 cloudSpawnPosition = venomSpawnPoint.position + venomSpawnPoint.forward * 8;
        Instantiate(venomCloudPrefab, cloudSpawnPosition, venomSpawnPoint.rotation);

        yield return new WaitForSeconds(sprayCooldown);
        canSpray = true;
        abilityCooldown = abilityTime;
    }

    // Istant dash
    IEnumerator VerticalDash()
    {
        isDashing = true;
        trailRenderer.emitting = true;
        dashSpeed = 1000;
        float dashTime = 0f;

        while (dashTime < dashDuration)
        {
            dashTime += Time.deltaTime;
            if (DashInterrupted())
                break;
            
            dashSound.Play();
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            InstantiateAndDestroy(dashSparkle, venomSpawnPoint.position, venomSpawnPoint.rotation, 1f);
            yield return null;
        }

        trailRenderer.emitting = false;
        isDashing = false;
        abilityCooldown = abilityTime;
    }

    // Check if the dash is interrupted
    private bool DashInterrupted()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, transform.forward, out hit, dashSpeed * Time.deltaTime) && hit.collider != null;
    }

    // Create and destroy an object
    private void InstantiateAndDestroy(GameObject prefab, Vector3 position, Quaternion rotation, float destroyTime)
    {
        GameObject instance = Instantiate(prefab, position, rotation);
        Destroy(instance, destroyTime);
    }
}
