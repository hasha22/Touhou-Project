using System.Collections;
using UnityEngine;
namespace KH
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance { get; private set; }

        [Header("Player Data")]
        public PlayableCharacterData characterData;
        [SerializeField] private int currentPlayerLives;

        [Header("Player Power")]
        public float currentPower;
        public bool hasConvertedPower = false;
        private float maxPower = 5.0f;
        [Space]
        [SerializeField] private int powerItemCount = 7;
        [SerializeField] private int spreadDegrees = 120;
        [SerializeField] private int powerDeathVelocity = 200;

        [Header("Player Respawning")]
        [SerializeField] private float respawnDelay = 0.5f;
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private float riseDistance = 1.5f;
        [SerializeField] private float riseSpeed = 2f;
        [SerializeField] private float lowerLimit = 10f;
        [SerializeField] private float upperLimit = 100f;
        private bool hasDied = false;
        public bool canPullItems = true;

        [Header("Player Invulnerability")]
        [SerializeField] private float invulnerabilityTime = 5f;
        [SerializeField] private float flickeringDelay = 0.1f;

        [Header("Auto-Collect Settings")]
        public float autoCollectY = 2.0f;
        public bool wasAboveAutoCollect = false;
        private float autoCollectTimer = 0f;
        public bool InCollectionZone => transform.position.y >= autoCollectY;

        [Header("Bomb Settings")]
        [SerializeField] private Transform bombParent;
        [SerializeField] private float bombDelay = 0.5f;
        [SerializeField] private GameObject bombPrefab;
        private bool bombLocked = false;
        private bool bombInput = false;

        [Header("Player Light Interaction")]
        public float damageMultiplier = 1.5f;
        public int scoreMultiplier = 2;
        private float multiplier1 = 1.5f;
        private int multiplier2 = 2;
        public bool inLight = false;
        private bool inShadow = true;
        // Add VFX and SFX later

        [Header("References")]
        [SerializeField] private Collider2D playerCollider;
        public Transform playerMagnetTransform;

        private SpriteRenderer spriteRenderer;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerCollider.isTrigger = true;
            multiplier1 = damageMultiplier;
            multiplier2 = scoreMultiplier;
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerMovement = GetComponent<PlayerMovement>();
        }
        private void Start()
        {
            UIManager.instance.UpdatePowerUI(currentPower);
            PlayerInputManager.instance.DisableInput();
            StartCoroutine(RespawnCoroutine());
        }
        private void Update()
        {
            bool isMaxPower = currentPower >= maxPower;

            Vector2 pos = transform.position;
            inLight = LightZoneManager.instance.IsInLight(pos);

            if (inLight)
            {
                damageMultiplier = multiplier1;
                scoreMultiplier = multiplier2;
            }
            else
            {
                damageMultiplier = 1f;
                scoreMultiplier = 1;
            }

            if (isMaxPower && !hasConvertedPower)
            {
                ItemManager.instance.ConvertAllPowerItems();
                hasConvertedPower = true;
            }
            else if (!isMaxPower)
            {
                hasConvertedPower = false;
            }

            bombInput = PlayerInputManager.instance.isBombing;

            bool nowAbove = transform.position.y >= autoCollectY;

            if (nowAbove && !wasAboveAutoCollect)
            {
                ItemManager.instance.AutoCollectAllItems();
            }

            if (nowAbove)
            {
                autoCollectTimer -= Time.deltaTime;
                if (autoCollectTimer <= 0f)
                {
                    ItemManager.instance.AutoCollectAllItems();
                    autoCollectTimer = 0.5f; // call every 0.2 seconds, tweak as needed
                }
            }

            if (!nowAbove && wasAboveAutoCollect)
            {
                ItemManager.instance.ResetItemPullRadius();
            }
            wasAboveAutoCollect = nowAbove;

        }
        private void FixedUpdate()
        {
            if (bombInput)
            {
                UseBomb(transform.position);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (playerCollider.enabled)
            {
                if (collision.CompareTag("Enemy"))
                {
                    Die();
                }
                if (collision.CompareTag("Enemy Bullet"))
                {
                    BulletController bulletController = collision.GetComponent<BulletController>();
                    BulletGrazing bulletGrazing = collision.GetComponent<BulletGrazing>();

                    if (!bulletGrazing.hasBeenGrazed && collision == bulletGrazing.grazeCollider)
                    {
                        bulletGrazing.hasBeenGrazed = true;
                        bulletGrazing.OnGrazed();
                    }
                    else if (collision == bulletController.bulletHitBox)
                    {
                        Die();
                        //ObjectPool.instance.ReturnToPool(collision.gameObject);
                    }
                }
            }
        }
        public void AddPower(float power)
        {
            if (currentPower < maxPower) { currentPower += power; }
            if (currentPower > maxPower) { currentPower = maxPower; }

            UIManager.instance.UpdatePowerUI(currentPower);
        }
        public void Die()
        {
            hasDied = true;
            currentPlayerLives--;
            AudioManager.instance.PlaySFX(AudioManager.instance.deathSFX, transform, 0.1f);
            UIManager.instance.RemoveLife();

            LosePower();

            if (currentPlayerLives == 0)
            {
                UIManager.instance.StartDeathScreenCoroutine();
                gameObject.SetActive(false);
                return;
            }
            PlayerInputManager.instance.DisableInput();
            if (gameObject.activeSelf)
            { StartCoroutine(RespawnCoroutine()); }
        }
        public void AddLife()
        {
            currentPlayerLives++;
            UIManager.instance.AddLife();
        }
        private void LosePower()
        {
            float t = Mathf.InverseLerp(playerMovement.minBounds.x, playerMovement.maxBounds.x, transform.position.x); // InverseLerp(a, b, t). Calculates where t lies in the (a,b) range, and returns a value between [0,1]
            float angle = Mathf.Lerp(lowerLimit, upperLimit, t); //Lerp(a, b, t). Formula: a + (b - a) * t; Example: a = 10, b = 100, t = 0.53 => angle = 57.7f
            float degrees = spreadDegrees / powerItemCount; // 17.14 increase in degrees each time a power item spawns

            for (int i = 0; i < powerItemCount; i++)
            {
                float radians = angle * Mathf.Deg2Rad; // Cos & Sin require radians
                Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

                GameObject powerItem = ItemManager.instance.InitializePlayerDeathItem(i, transform.position + new Vector3(0, 0.5f, 0));

                ItemController itemController = powerItem.GetComponent<ItemController>();
                itemController.LaunchItem(direction, powerDeathVelocity);

                angle += degrees;
            }

            float power = currentPower - 3.2f;
            if (power <= 0)
            { currentPower = 0; }
            else { currentPower = power; }
            UIManager.instance.UpdatePowerUI(currentPower);
        }
        public bool UseBomb(Vector3 position)
        {
            if (!CanBomb()) return false;
            StartCoroutine(UseBombCoroutine(position));
            return true;
        }
        public bool CanBomb()
        {
            return currentPower >= 1f && !bombLocked;
        }
        private IEnumerator UseBombCoroutine(Vector3 position)
        {
            bombLocked = true;
            currentPower -= 1f;
            UIManager.instance.UpdatePowerUI(currentPower);

            // might need to adjust player invulnerability later
            StartCoroutine(InvulnerabilityCoroutine());

            GameObject bomb = bombPrefab;
            Instantiate(bomb, position, Quaternion.identity, bombParent);
            bomb.SetActive(true);

            //Add VFX for bomb here

            yield return new WaitForSeconds(bombDelay);
            bombLocked = false;
        }
        private IEnumerator RespawnCoroutine()
        {
            spriteRenderer.enabled = false;
            playerCollider.enabled = false;
            canPullItems = false;
            if (hasDied) { yield return new WaitForSeconds(respawnDelay); }

            transform.position = respawnPoint.position;

            spriteRenderer.enabled = true;
            canPullItems = true;

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
            PlayerInputManager.instance.EnableInput();
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
}