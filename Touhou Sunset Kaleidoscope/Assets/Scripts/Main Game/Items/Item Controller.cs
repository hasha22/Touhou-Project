using UnityEngine;
namespace KH
{
    public class ItemController : MonoBehaviour
    {
        [Header("Item Settings")]
        [SerializeField] private float fallSpeed = 1f;
        [SerializeField] private float pullSpeed = 5f;
        [Space]
        public float currentPullRadius = 1.5f;
        [HideInInspector] public float autoCollectPullRadius = 20f;
        [HideInInspector] public float defaultPullRadius = 1.5f;
        public bool wasPulled = false;
        public bool canBePulled = false;

        [Header("References")]
        private Transform playerMagnet;
        private PlayerManager playerManager;
        private Rigidbody2D rb;
        private Transform playableArea;
        private Vector2 minBounds, maxBounds;

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

        [Header("Offscreen Settings")]
        [SerializeField] private GameObject indicatorPrefab;
        private GameObject indicatorInstance;
        [SerializeField] private Sprite greenIndicator;
        [SerializeField] private Sprite blueIndicator;
        [SerializeField] private Sprite redIndicator;
        public bool isAboveTop = false;

        [Header("Sprite Renderers")]
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer indicatorSpriteRenderer;

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            PlayerMovement playerMovement = playerManager.GetComponent<PlayerMovement>();
            playableArea = playerMovement.playableArea;
            indicatorSpriteRenderer = indicatorPrefab.GetComponent<SpriteRenderer>();

            playerMagnet = playerManager.playerMagnetTransform;
            launchTimerReset = launchTimer;

            BoxCollider2D area = playableArea.GetComponent<BoxCollider2D>();
            Bounds bounds = area.bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;

            indicatorInstance = Instantiate(indicatorPrefab);
            indicatorInstance.transform.SetParent(ItemManager.instance.topItemBar);
            indicatorInstance.SetActive(false);
        }
        private void OnDisable()
        {
            Destroy(indicatorInstance);
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
                ConstrainToBounds();
                return;
            }
            launchTimer = launchTimerReset;

            float distance = Vector2.Distance(transform.position, playerMagnet.position); // distance between player and item
            canBePulled = distance <= currentPullRadius;
            Vector3 itemPos = transform.position;

            isAboveTop = itemPos.y > ItemManager.instance.topItemBar.position.y;

            if (ItemManager.instance.isAutoCollecting && IsInPlayableArea(transform.position)) // auto-collected
            {
                transform.position = Vector2.MoveTowards(transform.position, playerMagnet.position, pullSpeed * Time.deltaTime);
                wasPulled = true;
            }
            else if (!canBePulled && !wasPulled) // free fall
            {
                transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
            }
            else if (!isAboveTop && playerManager.canPullItems) // normal pulling
            {
                transform.position = Vector2.MoveTowards(transform.position, playerMagnet.position, pullSpeed * Time.deltaTime);
            }


            if (isAboveTop) //displays offscreen indicator
            {
                indicatorInstance.SetActive(true);
                indicatorInstance.transform.position = new Vector3(itemPos.x, ItemManager.instance.topItemBar.position.y, itemPos.z);
            }
            else
            {
                indicatorInstance.SetActive(false);
            }
            ConstrainToBounds();
        }
        public void InitializeItem(ItemType type, int score, float power, int faith, Sprite itemSprite)
        {
            itemType = type;
            addedScore = score;
            addedPower = power;
            addedFaith = faith;
            UpdateSprites(itemSprite);
        }
        private void ConstrainToBounds()
        {
            Vector3 pos = transform.position;

            // Clamp X within bounds
            if (pos.x < minBounds.x)
                pos.x = minBounds.x;
            else if (pos.x > maxBounds.x)
                pos.x = maxBounds.x;

            transform.position = pos;
        }
        public bool IsInPlayableArea(Vector3 worldPos)
        {
            return worldPos.x >= minBounds.x && worldPos.y >= minBounds.y && worldPos.x < maxBounds.x && worldPos.y < maxBounds.y;
        }
        private void UpdateSprites(Sprite sprite)
        {
            if (!spriteRenderer) return;

            switch (itemType)
            {
                case ItemType.Score:
                    spriteRenderer.sprite = sprite;
                    indicatorSpriteRenderer.sprite = blueIndicator;
                    gameObject.tag = "Score";
                    break;
                case ItemType.Power:
                    spriteRenderer.sprite = sprite;
                    indicatorSpriteRenderer.sprite = redIndicator;
                    gameObject.tag = "Power";
                    break;
                case ItemType.Faith:
                    spriteRenderer.sprite = sprite;
                    indicatorSpriteRenderer.sprite = greenIndicator;
                    gameObject.tag = "Faith";
                    break;
                case ItemType.OneUp:
                    spriteRenderer.sprite = sprite;
                    gameObject.tag = "1 Up";
                    break;
            }
        }
        public void LaunchItem(Vector2 direction, float speed)
        {
            isLaunched = true;

            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction.normalized * speed;
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
                    FaithManager.instance.AddFaith(addedFaith);
                }
                if (gameObject.CompareTag("1 Up"))
                {
                    playerManager.AddLife();
                }
                wasPulled = false;
                currentPullRadius = defaultPullRadius;
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
}