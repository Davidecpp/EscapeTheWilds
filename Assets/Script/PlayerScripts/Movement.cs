using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    // Player prefab
    [SerializeField] private Transform target;

    public AudioSource walkingSound;
    
    // Particles
    [SerializeField] private ParticleSystem sandParticles;
    [SerializeField] private ParticleSystem waterParticles;
    [SerializeField] private ParticleSystem invincibleParticles;
    [SerializeField] private ParticleSystem healParticles;
    
    private GameObject invincibleParticlesObject;
    private bool invincibleEffectActive = false;
    private GameObject healParticlesObject;
    private bool healEffectActive = false;
    
    // Variables for movement and physics
    public float _rotSpeed = 15.0f; // Rotation speed of the player
    private float _slowSpeed; // Speed when walking on sand/water
    private float _gravity = -9.8f; // Gravity force applied to the player
    private float _terminalVelocity = -10.0f; // Maximum fall speed
    private float _minFall = -1.5f; // Minimum falling speed
    private float _vertSpeed; // Vertical speed, used for jumping and gravity
    private float _currentMoveSpeed; // Current movement speed based on terrain and effects
    private string _currentTerrainTag; // Tag of the terrain (sand, water, etc.)
    public float bounceStrenght = 2.0f; // Bounce strength on JumpPlatform
    
    // Boost variables
    public float boostSpeedMultiplier = 2.0f; // Multiplier for movement speed when boosted
    private bool isBoosted = false; // Flag to check if the player is boosted
    private float boostDuration = 5.0f; // Duration of the speed boost
    public GameObject boostAnimation; // Animation to play when boosted
    
    private CharacterController _characterController;
    private ControllerColliderHit _controllerCollider;
    private Inventory _inventory;
    private PlayerStats _playerStats;
    private ProvaCamera _camera;
    
    // Projectile variables
    [SerializeField] private float timer = 5; // Timer for projectile lifespan
    private float _bulletTime; // Timer used for bullet cooldown
    public GameObject bullet; // Bullet prefab
    public GameObject fireBullet; // Fire bullet prefab
    public Transform spawnPoint; // Bullet spawn point
    public float bulletSpeed = 10.0f; // Speed of the bullet
    private float _slowedSprintSpeed; // Speed when sprinting on slow terrain
    
    public bool megaJump; // Flag for the mega jump ability


    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>(); // Get the CharacterController component
        _playerStats = GetComponent<PlayerStats>(); // Get the PlayerStats component
        _camera = GetComponent<ProvaCamera>(); // Get the camera controller component
        _inventory = GetComponent<Inventory>(); // Get the Inventory component

        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats not found on the player.");
            return;
        }
        // Initialize movement variables
        _vertSpeed = _minFall;
        _currentMoveSpeed = _playerStats.GetMoveSpeed();
        _slowSpeed = _playerStats.GetMoveSpeed() / 2;
        _slowedSprintSpeed = _playerStats.GetMoveSpeed() / 2;
        sandParticles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if target is assigned
        if (target == null)
        {
            Debug.LogError("Target not assigned!"); // Error if target is missing
            return;
        }
        bool hitGround = false;
        RaycastHit hit = new RaycastHit(); 
        
        // Check if the player is hitting the ground
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_characterController.height + _characterController.radius) / 1.9f;
            hitGround = hit.distance <= check;

            if (hitGround)
            {
                // Call method to check terrain and show particles
                CheckTerrainAndShowParticles(hit);
                
                if (hit.collider.CompareTag("JumpPlatform"))
                {
                    // Apply bounce on JumpPlatform
                    _vertSpeed = _playerStats.GetJumpHeight() * bounceStrenght; 
                }
                else
                {
                    _vertSpeed = _minFall; // Reset fall speed
                }
            }
        }
        // Handle movement and jump
        HandleMovement(hitGround);
        HandleJump(hitGround, hit);
        
        // Handle projectile shooting when 'F' is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShootProjectile();
        }
        // Show active effects (invincibility, healing, etc.)
        ShowEffects();
    }
    
    // Handle player movement 
    private void HandleMovement(bool hitGround)
    {
        Vector3 movement = Vector3.zero;
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            UpdateMovementSpeed(); // Update movement speed based on terrain
            ChangeWalkingSound(); // Change walking sound depending on sprinting state

            movement.x = horInput * _currentMoveSpeed; // Horizontal movement
            movement.z = vertInput * _currentMoveSpeed;  // Vertical movement
            movement = Vector3.ClampMagnitude(movement, _currentMoveSpeed); // Ensure movement doesn't exceed max speed
            
            // Preserve rotation of the target while rotating towards movement direction
            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;
            
            // Rotate player towards the movement direction
            RotatePlayerToDirection(movement);
        }
        else if (walkingSound.isPlaying)
        {
            walkingSound.Stop(); // Stop walking sound if not moving
        }

        movement.y = _vertSpeed; // Apply vertical speed (gravity or jumping)
        movement *= Time.deltaTime; // Scale movement by frame time
        _characterController.Move(movement); // Move the player
    }
    
    // Rotate player towards movement direction
    private void RotatePlayerToDirection(Vector3 movement)
    {
        Quaternion direction = Quaternion.LookRotation(movement); // Calculate rotation based on movement
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, _rotSpeed * Time.deltaTime); // Smooth rotation
    }
    
    // Handle player jumping
    private void HandleJump(bool hitGround, RaycastHit hit)
    {
        // If on the ground and jump button is pressed, apply jump
        if (hitGround && Input.GetButtonDown("Jump") && !hit.collider.CompareTag("JumpPlatform"))
        {
            _vertSpeed = _playerStats.GetJumpHeight(); // Jump height is determined by player stats
        }
        else
        {
            ApplyGravity(); // Apply gravity if not on the ground
        }
    }
    
    // Apply gravity effect to the player
    private void ApplyGravity()
    {
        // If touching a collider, apply gravity based on normal
        if (_characterController.isGrounded)
        {
            if (_controllerCollider != null && Vector3.Dot(Vector3.zero, _controllerCollider.normal) < 0)
            {
                _vertSpeed = _controllerCollider.normal.y * _currentMoveSpeed;
            }
        }

        _vertSpeed += _gravity * 5 * Time.deltaTime; // Apply gravity
        if (_vertSpeed < _terminalVelocity)
        {
            _vertSpeed = _terminalVelocity; // Cap fall speed
        }
    }
    
    // Perform mega jump when activated
    public void PerformMegaJump()
    {
        if (_characterController.isGrounded)
        {
            _vertSpeed = _playerStats.GetJumpHeight() * 4; // Mega jump height
            megaJump = false;
        }
    }
    
    // Modify pitch for running sound
    private void ChangeWalkingSound()
    {
        // If sprinting, increase sound pitch
        if (Input.GetKey(KeyCode.LeftShift) && _camera.isSprinting)
        {
            walkingSound.pitch = 1.5f; 
        }
        else
        {
            walkingSound.pitch = 1.0f; // Reset pitch to normal
        }
        if (!walkingSound.isPlaying)
        {
            walkingSound.Play(); // Play walking sound if not playing
        }
    }
    
    // Handle particle effects (invincibility, healing)
    private void Effect(bool condition, ref bool active, ref GameObject go, ParticleSystem ps)
    {
        // Instantiate and destroy effects based on conditions
        if (condition && !active)
        {
            go = Instantiate(ps.gameObject, target.position, target.rotation); // Instantiate effect
            go.transform.SetParent(target); // Parent the effect to the player
            active = true; // Mark effect as active
        }

        if (!condition && active)
        {
            Destroy(go); // Destroy effect if condition is no longer true
            active = false;
        }
    }
    
    // Show active effects based on game manager's state
    private void ShowEffects()
    {
        Effect(GameManager.Instance.invincible, ref invincibleEffectActive, ref invincibleParticlesObject, invincibleParticles);
        Effect(GameManager.Instance.healing, ref healEffectActive,ref healParticlesObject, healParticles);
    }
    
    // Boosts the player's speed for a specified duration
    public void BoostSpeed(float duration)
    {
        if (!isBoosted) 
        {
            // Set the player as boosted and activate the boost animation
            isBoosted = true;
            boostAnimation.SetActive(true);
            
            // Start a coroutine to reset the speed after the specified duration
            StartCoroutine(ResetSpeedAfterDelay(duration)); 
        }
    }

    // Coroutine to reset the speed after the boost duration ends
    private IEnumerator ResetSpeedAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration); // Wait for the specified duration
        isBoosted = false; // Reset the boosted status
        boostAnimation.SetActive(false); // Deactivate the boost animation
    }
        
    // Check the terrain type (sand, water, etc.) and show corresponding particles
    void CheckTerrainAndShowParticles(RaycastHit hit)
    {
        string hitTag = hit.collider.tag; // Get the tag of the collided object
        
        // If the terrain is sand or water
        if (hitTag == "Sand" || hitTag == "Water")
        {
            // Only update if the terrain type has changed
            if (_currentTerrainTag != hitTag)
            {
                _currentTerrainTag = hitTag; // Set the current terrain type
                UpdateMovementSpeed(); // Update the player's movement speed
                
                // Show the corresponding particles
                if (hitTag == "Sand")
                {
                    ShowParticles(sandParticles); // Play sand particles
                }
                else if (hitTag == "Water")
                {
                    ShowParticles(waterParticles); // Play water particles
                }
            }
        }
        else
        {
            // If the terrain is neither sand nor water, stop the particles
            if (_currentTerrainTag != null)
            {
                _currentTerrainTag = null;
                UpdateMovementSpeed(); // Update movement speed when terrain is reset
                sandParticles.Stop(); // Stop sand particles
                waterParticles.Stop(); // Stop water particles
            }
        }
    }

    // Update the player's movement speed based on terrain and effects
    void UpdateMovementSpeed()
    {
        float baseSpeed;

        // Check if the player is on sand or water and adjust speed accordingly
        if (_currentTerrainTag == "Sand" || _currentTerrainTag == "Water")
        {
            // Slow down the player when walking on sand or water
            baseSpeed = Input.GetKey(KeyCode.LeftShift) && _camera.isSprinting ? _slowedSprintSpeed : _slowSpeed;
        }
        else
        {
            // Use the normal speed if not on special terrain
            baseSpeed = Input.GetKey(KeyCode.LeftShift) && _camera.isSprinting ? _playerStats.GetRunSpeed() : _playerStats.GetMoveSpeed();
        }

        // Apply boost speed multiplier if boosted
        if (isBoosted)
        {
            _currentMoveSpeed = baseSpeed * boostSpeedMultiplier; 
        }
        else
        {
            _currentMoveSpeed = baseSpeed; // Use normal speed
        }
    }

    // Shoot a bullet when 'F' is pressed
    void ShootProjectile()
    {
        // Ensure the player has bullets in the inventory before shooting
        if (_inventory != null && _inventory.GetBullets() > 0)
        {
            // Instantiate a new bullet at the spawn point
            GameObject bullet = Instantiate(this.bullet, spawnPoint.position, spawnPoint.rotation);
            Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();
            
            // Apply force to the bullet to shoot it forward
            if (bulletRig != null)
            {
                bulletRig.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse); // Add forward force
                bulletRig.angularVelocity = new Vector3(0, 90, 90); // Add rotation for effect
            }
            
            // Remove a bullet from the inventory
            _inventory.RemoveBullet();
            
            // Destroy the bullet after 2 seconds to avoid memory leaks
            Destroy(bullet, 2.0f);
        }
    }

    // Show particles if they are not already playing
    void ShowParticles(ParticleSystem particles)
    {
        if (!particles.isPlaying && particles != null)
        {
            particles.Play(); // Play the particles if not already active
        }
    }

    // On controller collision with another object
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _controllerCollider = hit; // Store the hit for further processing (if needed)
    }

    // On collision with another object
    private void OnCollisionEnter(Collision other)
    {
        // Check if the player collides with a cannon ball
        if (other.gameObject.CompareTag("CannonBall"))
        {
            Debug.Log("Cannon ball hit");
            _playerStats.ReduceHealth(2); // Reduce player's health by 2
        }
    }

    // On trigger collision with another object
    private void OnTriggerEnter(Collider other)
    {
        // If the player enters a dialogue area, trigger the dialogue
        if (other.gameObject.CompareTag("DialogueWall"))
        {
            FindObjectOfType<Dialogue>().SetDialogue(new string[] { "Choose an animal.", "Press E to interact." });
        }
        
        // If the player reaches the mountain top, update the mission progress
        if (other.gameObject.CompareTag("MountainTop"))
        {
            FindObjectOfType<MissionManager>().AddProgress("Mountain", 1);
        }
    }

}