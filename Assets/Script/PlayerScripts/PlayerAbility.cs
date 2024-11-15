using System;
using System.Collections;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public string characterName;

    // Dash
    public float dashSpeed = 20f;
    public float dashDuration = 0.5f;
    private bool isDashing;
    public TrailRenderer trailRenderer;
    public GameObject dashSparkle;
    public AudioSource dashSound;

    // Venom cloud
    public GameObject venomSprayPrefab;
    public GameObject venomCloudPrefab;
    public Transform venomSpawnPoint;
    public float sprayCooldown = 5f;
    private bool canSpray = true;
    public AudioSource spraySound;

    // Mega Jump
    public GameObject megaJumpParticles;
    public AudioSource jumpSound;

    // General ability management
    public float abilityTime = 3.0f;
    private float abilityCooldown;

    private CanvasManager _canvas;
    private Movement _movement;

    private void Start()
    {
        // Cache references
        _canvas = FindObjectOfType<CanvasManager>();
        _movement = FindObjectOfType<Movement>();
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
        
    }

    private void Update()
    {
        HandleCooldown();

        // Activate ability on key press
        if (Input.GetKeyDown(KeyCode.R) && abilityCooldown <= 0)
        {
            ActivateAbility();
        }
    }

    // Handles the ability cooldown and updates UI
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

        _canvas.UpdateAbilityCooldown(Mathf.Max(abilityCooldown, 0), abilityTime);
    }

    // Activates the player's ability based on the character's name
    private void ActivateAbility()
    {
        switch (characterName)
        {
            case "Deer":
                if (!isDashing)
                    StartCoroutine(PerformDash());
                break;
            case "Snake":
                if (canSpray)
                    StartCoroutine(SprayVenom());
                break;
            case "Rat":
                StartCoroutine(PerformMegaJump());
                break;
        }

        abilityCooldown = abilityTime;
    }

    // Performs a Mega Jump
    private IEnumerator PerformMegaJump()
    {
        InstantiateAndDestroy(megaJumpParticles, venomSpawnPoint.position, venomSpawnPoint.rotation, abilityTime);
        PlaySound(jumpSound);
        _movement.megaJump = true;
        _movement.PerformMegaJump();
        yield return new WaitForSeconds(0.5f);
    }

    // Sprays venom and spawns a venom cloud
    private IEnumerator SprayVenom()
    {
        canSpray = false;
        InstantiateAndDestroy(venomSprayPrefab, venomSpawnPoint.position, venomSpawnPoint.rotation, 0.5f);
        PlaySound(spraySound);

        yield return new WaitForSeconds(0.5f);

        Vector3 cloudSpawnPosition = venomSpawnPoint.position + venomSpawnPoint.forward * 8f;
        Instantiate(venomCloudPrefab, cloudSpawnPosition, venomSpawnPoint.rotation);

        yield return new WaitForSeconds(sprayCooldown);
        canSpray = true;
    }

    // Performs a dash
    private IEnumerator PerformDash()
    {
        if (DashInterrupted())
            yield break;

        isDashing = true;
        trailRenderer.emitting = true;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration && !DashInterrupted())
        {
            elapsedTime += Time.deltaTime;
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);

            InstantiateAndDestroy(dashSparkle, venomSpawnPoint.position, venomSpawnPoint.rotation, 1f);
            yield return null;
        }

        trailRenderer.emitting = false;
        isDashing = false;
    }

    // Checks if the dash is interrupted by an obstacle
    private bool DashInterrupted()
    {
        return Physics.Raycast(transform.position, transform.forward, out _, dashSpeed * Time.deltaTime);
    }

    // Instantiates a prefab and destroys it after a delay
    private void InstantiateAndDestroy(GameObject prefab, Vector3 position, Quaternion rotation, float destroyTime)
    {
        if (prefab == null) return;

        GameObject instance = Instantiate(prefab, position, rotation);
        Destroy(instance, destroyTime);
    }

    // Plays an audio source if available
    private void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
