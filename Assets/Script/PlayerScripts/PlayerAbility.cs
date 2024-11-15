using System.Collections;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public enum CharacterType { Deer, Snake, Rat } // Punto 1: Enum per il tipo di personaggio
    public CharacterType characterType;

    // Abilità
    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.5f;
    private bool isDashing = false;
    public TrailRenderer trailRenderer;
    public GameObject dashSparkle;
    public AudioSource dashSound;

    [Header("Venom Spray")]
    public GameObject venomSprayPrefab;
    public GameObject venomCloudPrefab;
    public Transform venomSpawnPoint;
    public float sprayDuration = 0.5f;
    public float sprayCooldown = 5f;
    public AudioSource spraySound;

    [Header("Mega Jump")]
    public GameObject megaJumpParticles;
    public AudioSource jumpSound;

    // Cooldown e gestione abilità
    public float abilityTime = 3.0f;
    private float abilityCooldown = 0;

    // Riferimenti
    private CanvasManager _canvas;
    private Movement _movement;

    private void Start()
    {
        // Punto 7: Migliore gestione delle dipendenze
        _canvas = FindObjectOfType<CanvasManager>();
        _movement = GetComponent<Movement>();

        if (trailRenderer != null)
            trailRenderer.emitting = false;
    }

    private void Update()
    {
        HandleCooldown();

        if (Input.GetKeyDown(KeyCode.R) && abilityCooldown <= 0)
        {
            ActivateAbility();
        }
    }

    // Punto 4: Gestione uniforme del cooldown
    private void HandleCooldown()
    {
        abilityCooldown = Mathf.Max(0, abilityCooldown - Time.deltaTime);
        _canvas.UpdateAbilityCooldown(abilityCooldown, abilityTime);
        _canvas.isAbiliting = abilityCooldown > 0;
    }

    private void ActivateAbility()
    {
        switch (characterType)
        {
            case CharacterType.Deer:
                if (!isDashing)
                    StartCoroutine(PerformDash());
                break;

            case CharacterType.Snake:
                if (abilityCooldown <= 0)
                    StartCoroutine(PerformVenomSpray());
                break;

            case CharacterType.Rat:
                if (abilityCooldown <= 0)
                    StartCoroutine(PerformMegaJump());
                break;
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        abilityCooldown = abilityTime;

        ToggleTrail(true);

        float dashTime = 0f;
        while (dashTime < dashDuration)
        {
            dashTime += Time.deltaTime;
            dashSound.Play();
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            InstantiateAndDestroy(dashSparkle, venomSpawnPoint.position, venomSpawnPoint.rotation, 1f);
            yield return null;
        }

        ToggleTrail(false);
        isDashing = false;
    }

    private IEnumerator PerformVenomSpray()
    {
        abilityCooldown = sprayCooldown;
        InstantiateAndDestroy(venomSprayPrefab, venomSpawnPoint.position, venomSpawnPoint.rotation, sprayDuration);
        spraySound.Play();

        yield return new WaitForSeconds(sprayDuration);

        Vector3 cloudSpawnPosition = venomSpawnPoint.position + venomSpawnPoint.forward * 8;
        Instantiate(venomCloudPrefab, cloudSpawnPosition, venomSpawnPoint.rotation);
    }

    private IEnumerator PerformMegaJump()
    {
        abilityCooldown = abilityTime;

        InstantiateAndDestroy(megaJumpParticles, venomSpawnPoint.position, venomSpawnPoint.rotation, abilityTime);
        jumpSound.Play();
        _movement.megaJump = true;

        yield return new WaitForSeconds(0.5f);

        _movement.PerformMegaJump();
    }

    // Helper per attivare/disattivare il TrailRenderer
    private void ToggleTrail(bool state)
    {
        if (trailRenderer != null)
            trailRenderer.emitting = state;
    }

    // Helper per creare e distruggere oggetti temporanei
    private void InstantiateAndDestroy(GameObject prefab, Vector3 position, Quaternion rotation, float destroyTime)
    {
        GameObject instance = Instantiate(prefab, position, rotation);
        Destroy(instance, destroyTime);
    }
}
