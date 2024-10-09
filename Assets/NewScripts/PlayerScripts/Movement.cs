using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    // Player prefab
    [SerializeField] private Transform target;
    
    // Partciles
    [SerializeField] private ParticleSystem sandParticles;
    [SerializeField] private ParticleSystem waterParticles;
    [SerializeField] private ParticleSystem invincibleParticles;
    [SerializeField] private ParticleSystem healParticles;
    
    // Variables
    public float _rotSpeed = 15.0f;
    private float _slowSpeed;
    private float _gravity = -9.8f;
    private float _terminalVelocity = -10.0f;
    private float _minFall = -1.5f;
    private float _vertSpeed;
    private float _currentMoveSpeed;
    private string _currentTerrainTag;
    public float bounceStrenght = 2.0f;
    
    // Boost
    public float boostSpeedMultiplier = 2.0f; 
    private bool isBoosted = false;
    private float boostDuration = 5.0f; 
    public GameObject boostAnimation;
    
    private CharacterController _characterController;
    private ControllerColliderHit _controllerCollider;
    private Inventory _inventory;
    private PlayerStats _playerStats;
    private ProvaCamera _camera;
    
    // projectile
    [SerializeField] private float timer = 5;
    private float _bulletTime;
    public GameObject bullet;
    public GameObject fireBullet;
    public Transform spawnPoint;
    public float bulletSpeed = 10.0f;
    private float _slowedSprintSpeed;


    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerStats = GetComponent<PlayerStats>();
        _camera = GetComponent<ProvaCamera>();
        _inventory = GetComponent<Inventory>();

        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats not found on the player.");
            return;
        }
        _vertSpeed = _minFall;
        _currentMoveSpeed = _playerStats.moveSpeed;
        _slowSpeed = _playerStats.moveSpeed / 2;
        _slowedSprintSpeed = _playerStats.runSpeed / 2;
        sandParticles.Stop();
        healParticles.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned!");
            return;
        }
        bool hitGround = false;
        RaycastHit hit = new RaycastHit(); 
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_characterController.height + _characterController.radius) / 1.9f;
            hitGround = hit.distance <= check;

            if (hitGround)
            {
                CheckTerrainAndShowParticles(hit);

                if (hit.collider.CompareTag("JumpPlatform"))
                {
                    _vertSpeed = _playerStats.jumpHeight * bounceStrenght; 
                }
                else
                {
                    _vertSpeed = _minFall;
                }
            }
        }

        Vector3 movement = Vector3.zero;
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            UpdateMovementSpeed();

            movement.x = horInput * _currentMoveSpeed;
            movement.z = vertInput * _currentMoveSpeed;
            movement = Vector3.ClampMagnitude(movement, _currentMoveSpeed);

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, _rotSpeed * Time.deltaTime);
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump") && !hit.collider.CompareTag("JumpPlatform"))
            {
                _vertSpeed = _playerStats.jumpHeight;
            }
        }
        else
        {
            if (_characterController.isGrounded)
            {
                if (_controllerCollider != null && Vector3.Dot(movement, _controllerCollider.normal) < 0)
                {
                    movement = _controllerCollider.normal * _currentMoveSpeed;
                }
                else if (_controllerCollider != null)
                {
                    movement += _controllerCollider.normal * _currentMoveSpeed;
                }
            }

            _vertSpeed += _gravity * 5 * Time.deltaTime;
            if (_vertSpeed < _terminalVelocity)
            {
                _vertSpeed = _terminalVelocity;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ShootProjectile();
        }

        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _characterController.Move(movement);

        ShowEffects();
    }
    
    // Show particles
    private void ShowEffects()
    {
        if (GameManager.Instance.invincible)
        {
            invincibleParticles.gameObject.SetActive(true);
        }
        else
        {
            invincibleParticles.gameObject.SetActive(false);
        }
        if (GameManager.Instance.healing)
        {
            StartCoroutine(HealEffect(1f));
        }
    }
    
    // Show heal particles
    private IEnumerator HealEffect(float seconds)
    {
        healParticles.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds); 
        healParticles.gameObject.SetActive(false);
        GameManager.Instance.healing = false;
    }
    
    public void BoostSpeed(float duration)
    {
        if (!isBoosted) 
        {
            isBoosted = true;
            boostAnimation.SetActive(true);
            StartCoroutine(ResetSpeedAfterDelay(duration)); 
        }
    }

    private IEnumerator ResetSpeedAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isBoosted = false;
        boostAnimation.SetActive(false);
    }
    
    // Check terrain tag, apply malus and show effects
    void CheckTerrainAndShowParticles(RaycastHit hit)
    {
        string hitTag = hit.collider.tag;
        if (hitTag == "Sand" || hitTag == "Water")
        {
            if (_currentTerrainTag != hitTag)
            {
                _currentTerrainTag = hitTag;
                UpdateMovementSpeed();
            
                if (hitTag == "Sand")
                {
                    ShowParticles(sandParticles);
                }
                else if (hitTag == "Water")
                {
                    ShowParticles(waterParticles);
                }
            }
        }
        else
        {
            if (_currentTerrainTag != null)
            {
                _currentTerrainTag = null;
                UpdateMovementSpeed();
                sandParticles.Stop();
                waterParticles.Stop();
            }
        }
    }
        
    // Modify player's speed
    void UpdateMovementSpeed()
    {
        float baseSpeed;

        if (_currentTerrainTag == "Sand" || _currentTerrainTag == "Water")
        {
            baseSpeed = Input.GetKey(KeyCode.LeftShift) && _camera.isSprinting ? _slowedSprintSpeed : _slowSpeed;
        }
        else
        {
            baseSpeed = Input.GetKey(KeyCode.LeftShift) && _camera.isSprinting ? _playerStats.runSpeed : _playerStats.moveSpeed;
        }

        if (isBoosted)
        {
            _currentMoveSpeed = baseSpeed * boostSpeedMultiplier; 
        }
        else
        {
            _currentMoveSpeed = baseSpeed; 
        }
    }
    
    void ShootProjectile()
    {
        // If it has bullets can shoot
        if (_inventory != null && _inventory.GetBullets() > 0)
        {
            GameObject bullet = Instantiate(this.bullet, spawnPoint.position, spawnPoint.rotation);
            Rigidbody bulletRig = bullet.GetComponent<Rigidbody>();
            
            if (bulletRig != null)
            {
                bulletRig.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse);
                bulletRig.angularVelocity = new Vector3(0, 90, 90);
            }
            
            _inventory.RemoveBullet();
            Destroy(bullet, 2.0f);
        }
    }
    
    // Generic particle player method
    void ShowParticles(ParticleSystem particles)
    {
        if (!particles.isPlaying && particles != null)
        {
            particles.Play();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _controllerCollider = hit;
    }
    
}
