using System;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float rotationSpeed = 50f;
    
    public float recoilForce = 5000f;        // Intensità del rinculo
    public float recoilDuration = 5f;   // Durata del rinculo

    private Vector3 originalPosition;     // Posizione originale per il ripristino
    private bool isRecoiling = false;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        HandleRotation();
        
        // SPACE to fire
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        // Ripristina il cannone alla posizione originale se in rinculo
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

    private void HandleRotation()
    {
        // SD for rotation
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
    }

    private void ApplyRecoil()
    {
        transform.localPosition += transform.right * recoilForce * Time.deltaTime; // Sposta all'indietro
        isRecoiling = true; // Attiva il rinculo
    }
}