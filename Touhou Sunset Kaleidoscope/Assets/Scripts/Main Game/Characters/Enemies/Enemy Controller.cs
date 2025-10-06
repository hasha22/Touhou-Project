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
        private AttackSequence currentAttackSequence;
        public bool startOnEnable = true;
        public Vector2 fireOriginOffset = Vector2.zero;

        [Header("Movement")]
        private EnemyMovementPattern currentMovementPattern;

        [Header("Bool")]
        [HideInInspector] public bool hasDied = false;

        [Header("References")]
        private SpriteRenderer spriteRenderer;
        private Coroutine attackCoroutine;
        private BoxCollider2D boxCollider2D;
        private Rigidbody2D rb;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            if (currentMovementPattern != null)
            {
                Vector2 delta = currentMovementPattern.GetNextPosition(transform, Time.deltaTime);
                Debug.Log(delta);
                rb.MovePosition(rb.position + delta);
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
            currentMovementPattern = data.movementPattern;
            currentMovementPattern.Initialize(transform);
        }
        public void InitializeAttackSequence(AttackSequence attackSequence)
        {
            currentAttackSequence = attackSequence;

            if (attackCoroutine != null)
                StopCoroutine(attackCoroutine);

            attackCoroutine = StartCoroutine(PlaySequence());
        }
        private IEnumerator PlaySequence()
        {
            int index = 0;
            if (currentAttackSequence.patternSteps.Count == 0)
                yield break;

            while (currentAttackSequence.loopPattern || index < currentAttackSequence.patternSteps.Count)
            {
                PatternStep step = currentAttackSequence.patternSteps[index];
                step.pattern.Fire(transform.position);

                yield return new WaitForSeconds(step.delayBeforeNextPattern);

                index++;

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
            { StopCoroutine(PlaySequence()); }

            ItemManager.instance.SpawnItem(enemyData.itemToSpawn, transform.position);
            ObjectPool.instance.ReturnToPool(gameObject);
        }

        public Enemy GetEnemyData()
        {
            return enemyData;
        }
    }
}