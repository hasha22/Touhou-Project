using System.Collections;
using UnityEngine;
namespace KH
{
    public class BossManager : MonoBehaviour
    {
        [Header("Boss Data")]
        public Boss bossData;
        [SerializeField] private int currentBossPhaseHealth;

        [Header("Phases")]
        public int currentPhaseIndex = 0;
        private int helperIndex = 0;
        private BossPhase currentPhase;

        [Header("Movement")]
        [SerializeField] private MovementSequence currentMovementSequence; // visualizer

        [Header("Boss Attacks")]
        [SerializeField] private AttackSequence currentAttackSequence; // visualizer

        [Header("Flags")]
        public bool isPaused = false;
        public bool isWaitingForDialogue = true;
        private bool phaseEndedEarly = false;
        private bool isInvulnerable = false;
        public bool isBossDefeated = false;
        public bool isInSpellCardPhase = false;

        [Header("References")]
        private BoxCollider2D boxCollider2D;
        private SpriteRenderer spriteRenderer;
        private Transform playableArea;
        private Vector2 minBounds, maxBounds;
        private PlayerManager playerManager;
        [HideInInspector] public Rigidbody2D rb;
        private Vector3 lastKnownPosition;

        [Header("Boss SFX")]
        [SerializeField] private AudioClip deathSFX;
        [SerializeField][Range(0, 1)] private float deathSFXVolume = 0.1f;

        [Header("Coroutines")]
        public Coroutine activeAttackPatternRoutine;
        private Coroutine phaseRoutine;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();

            playerManager = PlayerInputManager.instance.playerObject.GetComponent<PlayerManager>();
            PlayerMovement playerMovement = playerManager.GetComponent<PlayerMovement>();
            playableArea = playerMovement.playableArea;

            BoxCollider2D area = playableArea.GetComponent<BoxCollider2D>();
            Bounds bounds = area.bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;

            helperIndex = 0;
            activeAttackPatternRoutine = null;
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
            isPaused = false;
            isWaitingForDialogue = false;

            if (bossData.shouldHaveInitialDialogue)
            {
                isWaitingForDialogue = true;
                isPaused = true;
                DialogueManager.instance.StartDialogue(this.bossData.initialDialogueSequence);
            }
            else
            {
                UIManager.instance.InitializeBossUI(bossData);
                StartNextPhase();
            }
        }
        public void StartNextPhase()
        {
            if (currentPhaseIndex >= bossData.phases.Length)
            {
                OnBossDefeated();
                return;
            }

            if (bossData.shouldHaveMidFightDialogue)
            {
                //play midfight dialogue, stop all other logic.
            }
            currentPhase = bossData.phases[currentPhaseIndex];

            currentMovementSequence = bossData.phases[currentPhaseIndex].phaseMovementSequence;
            currentAttackSequence = bossData.phases[currentPhaseIndex].phaseAttackSequence;
            currentBossPhaseHealth = bossData.phases[currentPhaseIndex].phaseBossHealth;

            phaseRoutine = StartCoroutine(PhaseRoutine(currentPhase));
        }
        private IEnumerator PhaseRoutine(BossPhase phase)
        {
            phase.StartPhase(this);
            UIManager.instance.StartBossPhase(phase);

            isInSpellCardPhase = false;

            if (helperIndex % 2 == 0)
            {
                UIManager.instance.InitializeHealth(currentBossPhaseHealth, phase.phaseBossHealth);
                yield return StartCoroutine(BossInvulnerabilityCoroutine(phase));
            }
            if (phase.isSpellCard)
            {
                isInSpellCardPhase = true;
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

                BossSpellCardPhase bossSpellCardPhase = (BossSpellCardPhase)phase;
                if (!bossSpellCardPhase.playerHasDied)
                {
                    UIManager.instance.ShowSpellCardBonus(bossSpellCardPhase.spellCardBonus);
                    ScoreManager.instance.AwardSpellCardBonus(bossSpellCardPhase.spellCardBonus);
                    //show UI
                    // award score
                }
            }
            yield return StartCoroutine(BossInvulnerabilityCoroutine(phase));// small delay and boss invulnerability between phases
            currentPhaseIndex++;
            helperIndex++;
            StartNextPhase();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player Bullet") && !isPaused && !isWaitingForDialogue)
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
            StopAllCoroutines();
            // clear all bullets
            foreach (GameObject bullet in ObjectPool.instance.GetPooledEnemyBullets())
            {
                // play disappearing vfx
                ObjectPool.instance.ReturnToPool(bullet);
            }
            LightZoneManager.instance.RemoveAllZones();
            currentPhase.EndPhase(this);
            AudioManager.instance.PlaySFX(deathSFX, transform, deathSFXVolume);
            UIManager.instance.HideBossUI();
            Destroy(gameObject, 0.5f);
            StageManager.instance.isStageBossDefeated = true;

            if (bossData.shouldHaveDefeatedDialogue)
            {
                DialogueManager.instance.StartDialogue(bossData.defeatedDialogueSequence);
            }
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
        public void MakePlayerIneligibleForSpellCardBonus()
        {
            BossSpellCardPhase bossSpellCardPhase = (BossSpellCardPhase)currentPhase;
            bossSpellCardPhase.playerHasDied = true;
        }
        public void HideBoss()
        {
            isInvulnerable = true;
            spriteRenderer.enabled = false;
        }
        public void RevealBoss()
        {
            isInvulnerable = false;
            spriteRenderer.enabled = true;
        }
    }
}

