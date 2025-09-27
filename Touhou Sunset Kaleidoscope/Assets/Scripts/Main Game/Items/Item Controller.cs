using KH;
using UnityEngine;
public class ItemController : MonoBehaviour
{
    [Header("Item Speed")]
    [SerializeField] private float fallSpeed = 1f;
    [SerializeField] private float pullSpeed = 5f;
    [SerializeField] private float pullRadius = 1.5f;
    private bool canBePulled = false;

    [Header("References")]
    private Transform playerMagnet;
    private PlayerManager playerManager;

    [Header("Score")]
    // Do not modify these here. Check Item Manager
    public ItemType itemType;
    [SerializeField] private float addedPower;
    [SerializeField] private int addedScore;
    [SerializeField] private int addedFaith;

    [Header("Launch Settings")]
    [SerializeField] private bool isLaunched = false;
    [SerializeField] private float launchTimer = 3f;
    private float launchTimerReset;

    [Header("Sprite Renderer ")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        playerMagnet = GameObject.FindWithTag("Player Magnet").transform;
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        launchTimerReset = launchTimer;
    }

    private void Update()
    {
        if (isLaunched)
        {
            launchTimer -= Time.deltaTime;
            if (launchTimer <= 0f)
            {
                rb.linearVelocity = Vector3.zero;
                isLaunched = false;
            }
            return;
        }
        launchTimer = launchTimerReset;

        if (playerMagnet == null) return;

        float distance = Vector2.Distance(transform.position, playerMagnet.position);
        canBePulled = distance <= pullRadius;

        if (canBePulled)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerMagnet.position, pullSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }
    public void InitializeItem(ItemType type, int score, float power, int faith, Sprite itemSprite)
    {
        itemType = type;
        addedScore = score;
        addedPower = power;
        addedFaith = faith;
        UpdateSprite(itemSprite);
    }
    public void LaunchPowerItem(Vector2 direction, float speed)
    {
        isLaunched = true;

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
    }
    private void UpdateSprite(Sprite sprite)
    {
        if (!spriteRenderer) return;

        switch (itemType)
        {
            case ItemType.Score:
                spriteRenderer.sprite = sprite;
                gameObject.tag = "Score";
                break;
            case ItemType.Power:
                spriteRenderer.sprite = sprite;
                gameObject.tag = "Power";
                break;
            case ItemType.Faith:
                spriteRenderer.sprite = sprite;
                gameObject.tag = "Faith";
                break;
            case ItemType.OneUp:
                spriteRenderer.sprite = sprite;
                gameObject.tag = "1 Up";
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Magnet"))
        {
            if (gameObject.CompareTag("Power"))
            {
                playerManager.AddPower(addedPower);
            }
            if (gameObject.CompareTag("Score"))
            {
                ScoreManager.instance.AddScore(addedScore);
            }
            if (gameObject.CompareTag("Faith"))
            {
                ScoreManager.instance.AddFaith(addedFaith);
            }
            if (gameObject.CompareTag("1 Up"))
            {
                playerManager.AddLife();
            }
            ObjectPool.instance.ReturnToPool(gameObject);

        }
    }
    public float GetPower()
    {
        return addedPower;
    }
    public float GetFaith()
    {
        return addedFaith;
    }
    public int GetScore()
    {
        return addedScore;
    }
}
