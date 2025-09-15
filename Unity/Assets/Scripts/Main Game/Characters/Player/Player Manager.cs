using KH;
using System.Collections;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    [Header("Player Data")]
    public PlayableCharacterData characterData;
    [SerializeField] private int currentPlayerLives;
    [SerializeField] private float currentPower;

    [Header("Player Respawning")]
    [SerializeField] private float respawnDelay = 0.5f;
    [SerializeField] private float invulnerabilityTime = 4f;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float riseDistance = 1.5f;
    [SerializeField] private float riseSpeed = 2f;


    [Header("References")]
    [SerializeField] private Collider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    // Temporary, to be moved to Audio Manager
    [SerializeField] private AudioClip deathSFX;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
    }
    private void Start()
    {
        UIManager.instance.UpdatePowerUI(currentPower);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy Bullet") && playerCollider.enabled)
        {
            Die();
            ObjectPool.instance.ReturnToPool(collision.gameObject);
        }
    }
    public void AddPower(float power)
    {
        currentPower += power;
        UIManager.instance.UpdatePowerUI(currentPower);
    }
    private void Die()
    {
        currentPlayerLives--;
        AudioSource.PlayClipAtPoint(deathSFX, transform.position, 1.0f);
        UIManager.instance.RemoveLife();

        if (currentPlayerLives == 0)
        {
            gameObject.SetActive(false);
            UIManager.instance.StartDeathScreenCoroutine();
            return;
        }

        // Add method for losing power here
        StartCoroutine(RespawnCoroutine());
    }
    private IEnumerator RespawnCoroutine()
    {
        spriteRenderer.enabled = false;
        playerCollider.enabled = false;
        PlayerInputManager.instance.inputControls.Disable();

        yield return new WaitForSeconds(respawnDelay);

        transform.position = respawnPoint.position;

        spriteRenderer.enabled = true;

        StartCoroutine(InvulnerabilityCoroutine());

        Vector3 targetPos = respawnPoint.position + Vector3.up * riseDistance;
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                riseSpeed * Time.deltaTime
            );
            yield return null;
        }
        PlayerInputManager.instance.inputControls.Enable();
    }
    private IEnumerator InvulnerabilityCoroutine()
    {
        float elapsed = 0f;
        bool visible = true;

        // flickering
        while (elapsed < invulnerabilityTime)
        {
            visible = !visible;
            if (spriteRenderer != null) spriteRenderer.enabled = visible;

            yield return new WaitForSeconds(0.05f);
            elapsed += 0.2f;
        }

        if (spriteRenderer != null) spriteRenderer.enabled = true;
        playerCollider.enabled = true;
    }
}
