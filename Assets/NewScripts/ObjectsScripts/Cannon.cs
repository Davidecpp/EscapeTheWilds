using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour, IInteractible
{
    [SerializeField] private string prompt;
    [SerializeField] private bool shouldDisappear; 
    [SerializeField] private bool _bonusObj;

    public string InteractionPrompt => prompt;
    public bool bonusObj => _bonusObj;

    // Projectile
    public GameObject projectilePrefab;
    public ParticleSystem explosionParticles;
    public Transform firePoint;
    public float fireForce = 20f;
    public AudioSource explosionSound;

    public float rotationSpeed = 50f;
    private GameObject _player;
    private bool _inCannon;
    public GameObject camera;
    
    // Recoil
    public float recoilForce = 50f;
    public float recoilDuration = 1f;
    private Vector3 originalPosition;
    private bool isRecoiling = false;

    private Coroutine autoFireCoroutine;

    private void Start()
    {
        originalPosition = transform.localPosition;
        explosionSound = GetComponent<AudioSource>();
        _inCannon = false;
    }

    public bool Interact(Interactor interactor)
    {
        // Check if it interacts with player
        if (interactor.CompareTag("Player"))
        {
            _player = interactor.gameObject;
            _player.SetActive(false);
            camera.SetActive(true);
            _inCannon = true;

            // Stop the auto fire when player enters the cannon
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
        if (_inCannon)
        {
            HandleRotation();
            // SPACE to fire manually
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                camera.SetActive(false);
                _player.SetActive(true);
                _inCannon = false;

                // Start auto fire when player exits the cannon
                if (autoFireCoroutine == null)
                {
                    autoFireCoroutine = StartCoroutine(AutoFireCoroutine());
                }
            }
        }
        else
        {
            // Start auto fire if not already running
            if (autoFireCoroutine == null)
            {
                autoFireCoroutine = StartCoroutine(AutoFireCoroutine());
            }
        }

        // Reset cannon to original position
        if (isRecoiling)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime / recoilDuration);

            if (Vector3.Distance(transform.localPosition, originalPosition) < 0.01f)
            {
                transform.localPosition = originalPosition;
                isRecoiling = false;
            }
        }
    }

    private IEnumerator AutoFireCoroutine()
    {
        while (!_inCannon)
        {
            Fire();
            yield return new WaitForSeconds(3f); // Wait for 3 seconds between shots
        }
        autoFireCoroutine = null; // Reset coroutine reference when finished
    }

    private void HandleRotation()
    {
        // S-D for rotation
        float horizontal = Input.GetAxis("Horizontal");
        
        Vector3 rotation = new Vector3(0, horizontal, 0) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation, Space.Self);
    }

    private void Fire()
    {
        // Shoot cannon ball
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
        
        ApplyRecoil();
        Instantiate(explosionParticles, firePoint.position, firePoint.rotation);
        explosionSound.Play();
        Destroy(projectile, 2f);
    }

    private void ApplyRecoil()
    {
        transform.localPosition += transform.right * recoilForce * Time.deltaTime;
        isRecoiling = true;
    }
}
