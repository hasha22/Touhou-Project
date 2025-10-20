using System.Collections;
using UnityEngine;
namespace KH
{
    public class BossManager : MonoBehaviour
    {
        [Header("Boss Data")]
        [SerializeField] private Boss bossData;
        [SerializeField] private int currentBossPhaseHealth;

        [Header("Phases")]
        public int currentPhaseIndex = 0;
        private int helperIndex = 0;
        private BossPhase currentPhase;
        private bool phaseEndedEarly = false;
        private bool isInvulnerable = false;
        //[SerializeField] private float timerBeforeAttackSequenceBegins = 0f;

        [Header("Movement")]
        [SerializeField] private MovementSequence currentMovementSequence;

        [Header("Boss Attacks")]
        [SerializeField] private AttackSequence currentAttackSequence;

        [Header("References")]
        private BoxCollider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private Transform playableArea;
        private Vector2 minBounds, maxBounds;
        private PlayerManager playerManager;
        [HideInInspector] public Rigidbody2D rb;
        private PlayerShooter playerShooter;

        [Header("Coroutines")]
        private Coroutine phaseRoutine;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();

            playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            playerShooter = PlayerInputManager.instance.playerObject.GetComponent<PlayerShooter>();
            PlayerMovement playerMovement = playerManager.GetComponent<PlayerMovement>();
            playableArea = playerMovement.playableArea;

            BoxCollider2D area = playableArea.GetComponent<BoxCollider2D>();
            Bounds bounds = area.bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;

            helperIndex = 0;
        }
        private void Update()
        {
            if (!IsInPlayableArea(transform.position))
            {
                if (phaseRoutine != null)
                {
                    StopCoroutine(PhaseRoutine(currentPhase));
                }
            }
            if (bossData != null)
            {
                UIManager.instance.UpdateTimer();
            }
        }
        public void InitializeBoss(Boss bossData)
        {
            this.bossData = bossData;
            transform.position = bossData.spawnPoint;
            transform.rotation = Quaternion.identity;

            boxCollider2D.size = bossData.colliderSize;
            boxCollider2D.offset = bossData.colliderOffset;
            spriteRenderer.sprite = bossData.bossSprite;

            spriteRenderer.enabled = true;

            StartNextPhase();
            UIManager.instance.InitializeBossUI(bossData);
        }
        public void StartNextPhase()
        {
            if (currentPhaseIndex >= bossData.phases.Length)
            {
                OnBossDefeated();
                return;
            }

            currentPhase = bossData.phases[currentPhaseIndex];
            currentBossPhaseHealth = bossData.phases[currentPhaseIndex].phaseBossHealth;

            phaseRoutine = StartCoroutine(PhaseRoutine(currentPhase));
        }
        private IEnumerator PhaseRoutine(BossPhase phase)
        {
            phase.StartPhase(this);
            UIManager.instance.StartBossPhase(phase);

            if (helperIndex % 2 == 0)
            {
                UIManager.instance.InitializeHealth(currentBossPhaseHealth, phase.phaseBossHealth);
                yield return StartCoroutine(BossInvulnerabilityCoroutine(phase));
            }
            if (phase.isSpellCard)
            {
                UIManager.instance.PlaySpellCardCutIn(spriteRenderer.sprite, phase.phaseName);
            }

            float timer = 0f;
            phaseEndedEarly = false;

            while (timer < phase.duration && !phaseEndedEarly)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            phase.EndPhase(this);
            if (phase.isSpellCard)
            {
                UIManager.instance.OnLifeLost(bossData.phases.Length / 2);
            }
            yield return StartCoroutine(BossInvulnerabilityCoroutine(phase));// small delay and boss invulnerability between phases
            currentPhaseIndex++;
            helperIndex++;
            StartNextPhase();
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
            if (isInvulnerable) return;

            currentBossPhaseHealth -= damage;
            ScoreManager.instance.AddScore(bossData.hitScore * damage);
            UIManager.instance.UpdateHealthImmediate(currentBossPhaseHealth);

            if (currentBossPhaseHealth <= 0 && !phaseEndedEarly)
            {
                phaseEndedEarly = true;
                currentBossPhaseHealth = 0;
                currentPhase.EndPhase(this);
            }

        }
        private void OnBossDefeated()
        {
            // trigger items drops, spell card bonus, etc.

            // clear all bullets
            foreach (GameObject bullet in ObjectPool.instance.GetPooledEnemyBullets())
            {
                // play disappearing vfx
                ObjectPool.instance.ReturnToPool(bullet);
            }
            Debug.Log("Boss Defeated!");
            UIManager.instance.HideBossUI();
            StopAllCoroutines();
            Destroy(gameObject, 0.5f);
        }
        private bool IsInPlayableArea(Vector3 worldPos)
        {
            return worldPos.x >= minBounds.x && worldPos.y >= minBounds.y && worldPos.x < maxBounds.x && worldPos.y < maxBounds.y;
        }
        private IEnumerator BossInvulnerabilityCoroutine(BossPhase phase)
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(phase.delayBeforeNextPhase);
            isInvulnerable = false;
        }
    }
}

