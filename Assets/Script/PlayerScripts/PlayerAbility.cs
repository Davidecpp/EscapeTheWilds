using System.Collections;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    // Enum to define character type
    public enum CharacterType { Deer, Snake, Rat, Monkey } 
    public CharacterType characterType; // Selected character type

    // Dash ability variables
    [Header("Dash")]
    public float dashSpeed = 20f; // Speed of the dash
    public float dashDuration = 0.5f; // Duration of the dash
    private bool isDashing = false; // Is the player currently dashing?
    public TrailRenderer trailRenderer; // Trail effect during the dash
    public GameObject dashSparkle; // Visual effect for the dash
    public AudioSource dashSound; // Sound effect for the dash

    // Venom spray ability variables
    [Header("Venom Spray")]
    public GameObject venomSprayPrefab; // Prefab for venom spray
    public GameObject venomCloudPrefab; // Prefab for venom cloud
    public Transform venomSpawnPoint; // Position where venom is spawned
    public float sprayDuration = 0.5f; // Duration of the venom spray
    public float sprayCooldown = 5f; // Cooldown for venom spray
    public AudioSource spraySound; // Sound effect for venom spray

    // Mega jump ability variables
    [Header("Mega Jump")]
    public GameObject megaJumpParticles; // Particle effect for mega jump
    public AudioSource jumpSound; // Sound effect for mega jump
    
    [Header("Banana Spam")]
    public GameObject bananaPrefab; // Banana prefab
    public Transform bananaSpawnPoint; // Banana spawn point
    public int bananaCount = 10; // Banana bullets avalaible
    public float bananaInterval = 0.2f; // Shoot interval
    public AudioSource bananaSound; // Sound effect for BananaSpam

    // Ability cooldown variables
    public float abilityTime = 3.0f; // General cooldown time for abilities
    private float abilityCooldown = 0; // Current cooldown time remaining

    // References to other components
    private CanvasManager _canvas; // Reference to UI manager
    private Movement _movement; // Reference to player movement script

    private void Start()
    {
        // Initialize references to dependent components
        _canvas = FindObjectOfType<CanvasManager>();
        _movement = GetComponent<Movement>();

        // Ensure trail is initially disabled
        if (trailRenderer != null)
            trailRenderer.emitting = false;
    }

    private void Update()
    {
        HandleCooldown(); // Update the cooldown timer

        // Activate ability when the 'R' key is pressed and cooldown is ready
        if (Input.GetKeyDown(KeyCode.R) && abilityCooldown <= 0)
        {
            ActivateAbility();
        }
    }

    // Manage the cooldown timer
    private void HandleCooldown()
    {
        // Reduce cooldown over time, ensuring it doesn't go below zero
        abilityCooldown = Mathf.Max(0, abilityCooldown - Time.deltaTime);

        // Update UI with the current cooldown state
        _canvas.UpdateAbilityCooldown(abilityCooldown, abilityTime);
        _canvas.isAbiliting = abilityCooldown > 0; // Update UI state
    }

    // Activate the appropriate ability based on character type
    private void ActivateAbility()
    {
        switch (characterType)
        {
            case CharacterType.Deer:
                if (!isDashing)
                    StartCoroutine(PerformDash()); // Dash if Deer
                break;

            case CharacterType.Snake:
                if (abilityCooldown <= 0)
                    StartCoroutine(PerformVenomSpray()); // VenomSpray if Snake
                break;

            case CharacterType.Rat:
                if (abilityCooldown <= 0)
                    StartCoroutine(PerformMegaJump()); // MegaJump if Rat
                break;
            
            case CharacterType.Monkey:
                if (abilityCooldown <= 0)
                    StartCoroutine(BananaSpam()); // BananaSpam if Monkey
                break;
        }
    }

    // Perform the dash ability
    private IEnumerator PerformDash()
    {
        isDashing = true;
        abilityCooldown = abilityTime;

        // Enable trail effect
        ToggleTrail(true);

        float dashTime = 0f;
        while (dashTime < dashDuration)
        {
            dashTime += Time.deltaTime;
            dashSound.Play(); // Play dash sound
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime); // Move forward
            InstantiateAndDestroy(dashSparkle, venomSpawnPoint.position, venomSpawnPoint.rotation, 1f); // Create visual effect
            yield return null;
        }

        // Disable trail effect
        ToggleTrail(false);
        isDashing = false;
    }

    // Perform the venom spray ability
    private IEnumerator PerformVenomSpray()
    {
        abilityCooldown = sprayCooldown;

        // Create venom spray effect
        InstantiateAndDestroy(venomSprayPrefab, venomSpawnPoint.position, venomSpawnPoint.rotation, sprayDuration);
        spraySound.Play(); // Play spray sound

        // Wait for the spray duration
        yield return new WaitForSeconds(sprayDuration);

        // Create venom cloud effect
        Vector3 cloudSpawnPosition = venomSpawnPoint.position + venomSpawnPoint.forward * 8;
        Instantiate(venomCloudPrefab, cloudSpawnPosition, venomSpawnPoint.rotation);
    }

    // Perform the mega jump ability
    private IEnumerator PerformMegaJump()
    {
        abilityCooldown = abilityTime;

        // Create jump particles and play sound
        InstantiateAndDestroy(megaJumpParticles, venomSpawnPoint.position, venomSpawnPoint.rotation, abilityTime);
        jumpSound.Play();
        _movement.megaJump = true; // Enable mega jump in movement script

        yield return new WaitForSeconds(0.5f); // Retard the jump

        // Execute the mega jump
        _movement.PerformMegaJump();
    }
    
    // Perform the banana spam ability
    private IEnumerator BananaSpam()
    {
        abilityCooldown = abilityTime;

        for (int i = 0; i < bananaCount; i++)
        {
            // Spawn a banana and apply forward force
            GameObject banana = Instantiate(bananaPrefab, bananaSpawnPoint.position, bananaSpawnPoint.rotation);
            Rigidbody rb = banana.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(bananaSpawnPoint.forward * 15f, ForceMode.Impulse);
            }

            // Play sound effect
            bananaSound.Play();

            // Destroy banana after a few seconds
            Destroy(banana, 3f);

            // Wait for the interval before spawning the next banana
            yield return new WaitForSeconds(bananaInterval);
            
            // Stop sound effect
            bananaSound.Stop();
        }
    }

    // Helper method to toggle the trail renderer state
    private void ToggleTrail(bool state)
    {
        if (trailRenderer != null)
            trailRenderer.emitting = state;
    }

    // Helper method to instantiate and destroy temporary objects
    private void InstantiateAndDestroy(GameObject prefab, Vector3 position, Quaternion rotation, float destroyTime)
    {
        GameObject instance = Instantiate(prefab, position, rotation); // Instantiate the object
        Destroy(instance, destroyTime); // Destroy after specified time
    }
}
