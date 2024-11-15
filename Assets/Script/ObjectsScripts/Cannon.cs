using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour, IInteractible
{
    // Interaction settings
    [SerializeField] private string prompt;         // Interaction prompt shown to the player
    [SerializeField] private bool shouldDisappear;  // If true, the cannon will disappear after use
    [SerializeField] private bool _bonusObj;        // Bonus object flag for special mechanics

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;

    // Projectile and firing settings
    public GameObject projectilePrefab;             // Cannonball prefab
    public ParticleSystem explosionParticles;       // Particle system for explosion effect
    public Transform firePoint;                     // Point where the projectile is spawned
    public float fireForce = 20f;                   // Force applied to the projectile
    public AudioSource explosionSound;              // Sound played on firing

    // Cannon rotation settings
    public float rotationSpeed = 50f;               // Speed of cannon rotation
    private GameObject _player;                     // Reference to the player object
    private bool _inCannon;                         // Flag to track if the player is in the cannon
    public GameObject camera;                       // Camera to activate when player enters cannon

    // Recoil settings
    public float recoilForce = 50f;                 // Force applied for cannon recoil
    public float recoilDuration = 1f;               // Duration of the recoil effect
    private Vector3 originalPosition;               // Initial position of the cannon
    private bool isRecoiling = false;               // Flag for recoil in progress

    private Coroutine autoFireCoroutine;            // Coroutine for automatic firing

     private void Start()
    {
        // Save the cannon's initial position and get the AudioSource
        originalPosition = transform.localPosition;
        explosionSound = GetComponent<AudioSource>();
        _inCannon = false; // Player starts outside the cannon
    }

    public bool Interact(Interactor interactor)
    {
        // Handle interaction when the player enters the cannon
        if (interactor.CompareTag("Player"))
        {
            _player = interactor.gameObject; // Assign the player object
            _player.SetActive(false);       // Deactivate the player
            camera.SetActive(true);         // Activate the cannon's camera
            _inCannon = true;

            // Stop auto-firing while the player controls the cannon
            if (autoFireCoroutine != null)
            {
                StopCoroutine(autoFireCoroutine);
                autoFireCoroutine = null;
            }
            return true;
        }

        return false;
    }

    private void Update()
    {
        // Handle cannon functionality when the player is inside
        if (_inCannon)
        {
            HandleRotation();

            // Fire the cannon manually with the SPACE key
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }

            // Exit the cannon with the E key
            if (Input.GetKeyDown(KeyCode.E))
            {
                camera.SetActive(false);   // Deactivate the cannon camera
                _player.SetActive(true);  // Reactivate the player
                _inCannon = false;

                // Resume auto-firing if applicable
                if (autoFireCoroutine == null)
                {
                    autoFireCoroutine = StartCoroutine(AutoFireCoroutine());
                }
            }
        }
        else
        {
            // Start auto-firing if it's not running and the player isn't controlling the cannon
            if (autoFireCoroutine == null)
            {
                autoFireCoroutine = StartCoroutine(AutoFireCoroutine());
            }
        }

        // Reset the cannon's position during recoil
        if (isRecoiling)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime / recoilDuration);

            // Stop recoil if the cannon returns to its original position
            if (Vector3.Distance(transform.localPosition, originalPosition) < 0.01f)
            {
                transform.localPosition = originalPosition;
                isRecoiling = false;
            }
        }
    }

    private IEnumerator AutoFireCoroutine()
    {
        // Automatic firing when the player isn't in the cannon
        while (!_inCannon)
        {
            Fire();
            yield return new WaitForSeconds(3f); // Delay between automatic shots
        }
        autoFireCoroutine = null; // Clear coroutine reference
    }

    private void HandleRotation()
    {
        // Rotate the cannon using horizontal input (e.g., S and D keys)
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 rotation = new Vector3(0, horizontal, 0) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation, Space.Self);
    }

    private void Fire()
    {
        // Instantiate and fire the cannonball
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);

        ApplyRecoil(); // Apply recoil to the cannon
        Instantiate(explosionParticles, firePoint.position, firePoint.rotation); // Create explosion effect
        explosionSound.Play(); // Play sound effect

        // Destroy the projectile after 2 seconds
        Destroy(projectile, 2f);
    }

    private void ApplyRecoil()
    {
        // Move the cannon backward to simulate recoil
        transform.localPosition += transform.right * recoilForce * Time.deltaTime;
        isRecoiling = true; // Enable recoil tracking
    }
}
