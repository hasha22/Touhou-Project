using KH;
using System.Collections;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private Enemy enemyData;
    [SerializeField] private int currentHealth;

    [Header("Shooting")]
    private AttackSequence attackSequence;
    public bool startOnEnable = true;
    public Vector2 fireOriginOffset = Vector2.zero;

    private Coroutine playCoroutine;

    private void OnEnable()
    {
        currentHealth = enemyData.enemyHealth;
        attackSequence = enemyData.attackSequence;

        if (startOnEnable && attackSequence != null)
            playCoroutine = StartCoroutine(PlaySequence());
    }
    public void StartSequence()
    {
        if (playCoroutine == null) playCoroutine = StartCoroutine(PlaySequence());
    }
    public void StopSequence()
    {
        if (playCoroutine != null) { StopCoroutine(playCoroutine); playCoroutine = null; }
    }

    private IEnumerator PlaySequence()
    {
        do
        {
            foreach (var step in attackSequence.patternSteps)
            {
                step.pattern.Fire((Vector2)transform.position + fireOriginOffset);

                yield return new WaitForSeconds(step.delayBeforeNextPattern);
            }
        } while (attackSequence.loopPattern);
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
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ScoreManager.instance.AddScore(enemyData.hitScore * damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // add death sound,vfx

        ScoreManager.instance.AddScore(enemyData.deathScore);
        gameObject.SetActive(false);
        ItemManager.instance.SpawnGreatScoreItem(transform.position);
    }
}
