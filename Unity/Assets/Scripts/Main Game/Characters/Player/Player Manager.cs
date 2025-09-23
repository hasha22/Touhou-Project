using KH;
using System.Collections;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    [Header("Player Data")]
    public PlayableCharacterData characterData;
    [SerializeField] private int currentPlayerLives;

    [Header("Player Power")]
    public float currentPower;
    private float maxPower = 5.0f;

    [Header("Player Respawning")]
    [SerializeField] private float respawnDelay = 0.5f;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float riseDistance = 1.5f;
    [SerializeField] private float riseSpeed = 2f;
    private bool hasDied = false;

    [Header("Player Invulnerability")]
    [SerializeField] private float invulnerabilityTime = 5f;
    [SerializeField] private float flickeringDelay = 0.1f;

    [Header("References")]
    [SerializeField] private Collider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        UIManager.instance.UpdatePowerUI(currentPower);
        StartCoroutine(RespawnCoroutine());
    }
    private void Update()
    {
        // not good enough
        if (currentPower == maxPower)
        {
            ItemManager.instance.ConvertAllPowerItems();
        }
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
        if (currentPower < maxPower) { currentPower += power; }
        if (currentPower > maxPower) { currentPower = maxPower; }

        UIManager.instance.UpdatePowerUI(currentPower);
    }
    private void Die()
    {
        hasDied = true;
        currentPlayerLives--;
        AudioManager.instance.PlaySFX(AudioManager.instance.deathSFX, transform, 1f);
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
    public void AddLife()
    {
        currentPlayerLives++;
        UIManager.instance.AddLife();
    }
    private IEnumerator RespawnCoroutine()
    {
        spriteRenderer.enabled = false;
        playerCollider.enabled = false;
        PlayerInputManager.instance.inputControls.Disable();

        if (hasDied) { yield return new WaitForSeconds(respawnDelay); }

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

        Color invulnerabilityColor = new Color(0.49f, 0.57f, 0.93f);
        spriteRenderer.color = invulnerabilityColor;

        // flickering
        while (elapsed < invulnerabilityTime)
        {
            visible = !visible;
            if (spriteRenderer != null) spriteRenderer.enabled = visible;

            yield return new WaitForSeconds(flickeringDelay);
            elapsed += 0.2f;
        }

        if (spriteRenderer != null) spriteRenderer.enabled = true;
        playerCollider.enabled = true;
        spriteRenderer.color = Color.white;
    }
}
