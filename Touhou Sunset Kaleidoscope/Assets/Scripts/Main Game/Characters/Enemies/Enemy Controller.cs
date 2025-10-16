using System.Collections;
using UnityEngine;
namespace KH
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Enemy Stats")]
        [SerializeField] private Enemy enemyData;
        [SerializeField] private int currentHealth;

        [Header("Shooting")]
        public AttackSequence currentAttackSequence;
        public Vector2 fireOriginOffset = Vector2.zero;
        public float timer = 0f;

        [Header("Movement")]
        public MovementSequence currentMovementSequence;

        [Header("Bool")]
        [HideInInspector] public bool hasDied = false;

        [Header("References")]
        private SpriteRenderer spriteRenderer;
        private Coroutine attackCoroutine;
        private BoxCollider2D boxCollider2D;
        private Rigidbody2D rb;
        private Transform playableArea;
        private Vector2 minBounds, maxBounds;
        private PlayerManager playerManager;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();

            playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            PlayerMovement playerMovement = playerManager.GetComponent<PlayerMovement>();
            playableArea = playerMovement.playableArea;

            BoxCollider2D area = playableArea.GetComponent<BoxCollider2D>();
            Bounds bounds = area.bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;
        }
        private void OnEnable()
        {
            if (currentMovementSequence != null)
            {
                StartCoroutine(MovementSequence());
            }
        }
        private void OnDisable()
        {
            attackCoroutine = null;
        }

        private void Update()
        {
            // Attacking is only enabled once they enter the playable area
            if (!IsInPlayableArea(transform.position))
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(AttackSequence());
                }
            }
            else if (attackCoroutine == null)
            {
                timer += Time.deltaTime;

                if (timer > enemyData.delayBeforeAttack)
                {
                    timer = 0;
                    attackCoroutine = StartCoroutine(AttackSequence());
                }
            }
        }
        public void InitializeEnemy(Enemy data, Vector2 spawnPosition)
        {
            enemyData = data;
            transform.position = spawnPosition;
            transform.rotation = Quaternion.identity;

            boxCollider2D.size = data.colliderSize;
            boxCollider2D.offset = data.colliderOffset;

            spriteRenderer.sprite = data.enemySprite;
            currentHealth = data.enemyHealth;
            currentAttackSequence = data.attackSequence;

            currentMovementSequence = data.movementSequence;

            spriteRenderer.enabled = true;
        }
        public void InitializeAttackSequence(AttackSequence attackSequence)
        {
            currentAttackSequence = attackSequence;

            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);

        }
        private IEnumerator MovementSequence()
        {
            int index = 0;
            if (currentMovementSequence.movementSteps.Count == 0)
                yield break;

            // runs each step in the movement sequence
            while (index < currentMovementSequence.movementSteps.Count || currentMovementSequence.loopSequence)
            {
                MovementStep step = currentMovementSequence.movementSteps[index];
                float elapsed = 0f;

                Vector2 startPosition = rb.position;

                while (elapsed < step.duration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / step.duration);

                    // get total movement
                    Vector2 totalOffset = step.pattern.GetTotalMovement(transform, step.duration);
                    Vector2 targetPosition = startPosition + totalOffset;

                    // lerp to target position
                    rb.MovePosition(Vector2.Lerp(startPosition, targetPosition, t));

                    yield return null;
                }
                // final check to ensure enemy is on target
                Vector2 finalOffset = step.pattern.GetTotalMovement(transform, step.duration);
                rb.MovePosition(startPosition + finalOffset);

                if (step.delayBeforeNext > 0)
                    yield return new WaitForSeconds(step.delayBeforeNext);

                index++;
                if (index >= currentMovementSequence.movementSteps.Count)
                {
                    if (currentMovementSequence.loopSequence)
                        index = 0;
                    else
                        break;
                }
            }
        }
        private IEnumerator AttackSequence()
        {
            int index = 0;
            if (currentAttackSequence.patternSteps.Count == 0)
                yield break;

            // runs each step in the attack sequence
            while (currentAttackSequence.loopPattern || index < currentAttackSequence.patternSteps.Count)
            {
                PatternStep step = currentAttackSequence.patternSteps[index];
                step.pattern.Fire(transform.position);

                // waits before firing next attack
                yield return new WaitForSeconds(step.delayBeforeNextPattern);

                index++;

                // runs again if pattern loops
                if (index >= currentAttackSequence.patternSteps.Count)
                {
                    if (currentAttackSequence.loopPattern)
                        index = 0;
                    else
                        break;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player Bullet"))
            {
                BulletController bullet = collision.GetComponent<BulletController>();
                TakeDamage(bullet.bulletDamage);
                ObjectPool.instance.ReturnToPool(collision.gameObject);
            }
            else if (collision.CompareTag("AfterImage"))
            {
                ObjectPool.instance.ReturnToPool(collision.gameObject);
            }
        }
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            ScoreManager.instance.AddScore(enemyData.hitScore * damage);
            if (currentHealth <= 0 && !hasDied)
            {
                hasDied = true;
                Die();
            }
        }
        private void Die()
        {
            // add death sound,vfx

            ScoreManager.instance.AddScore(enemyData.deathScore);
            if (attackCoroutine != null)
            { StopCoroutine(AttackSequence()); }

            ItemManager.instance.SpawnItem(enemyData.itemToSpawn, transform.position);
            ObjectPool.instance.ReturnToPool(gameObject);
        }
        public bool IsInPlayableArea(Vector3 worldPos)
        {
            return worldPos.x >= minBounds.x && worldPos.y >= minBounds.y && worldPos.x < maxBounds.x && worldPos.y < maxBounds.y;
        }
        public Enemy GetEnemyData()
        {
            return enemyData;
        }
    }
}