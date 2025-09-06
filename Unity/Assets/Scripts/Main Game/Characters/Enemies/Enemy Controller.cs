using KH;
using System.Collections;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float currentHealth;

    [Header("Shooting")]
    [SerializeField] private AttackSequence attackSequence;
    public bool startOnEnable = true;
    public Vector2 fireOriginOffset = Vector2.zero;

    private Coroutine playCoroutine;

    private void OnEnable()
    {
        currentHealth = maxHealth;

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

            foreach (var step in attackSequence.steps)
            {
                Debug.Log("Skib");
                step.pattern.Fire((Vector2)transform.position + fireOriginOffset);

                yield return new WaitForSeconds(step.delayAfter);
            }
        } while (attackSequence.loop);
    }

    private void Fire()
    {/*
        foreach (var offset in shotPattern.spawnOffsets)
        {
            Vector3 spawnPosition = transform.position + (Vector3)offset;
            GameObject bulletObj = ObjectPool.instance.GetPooledEnemyObject();

            if (bulletObj != null)
            {
                bulletObj.transform.position = spawnPosition;
                BulletController bullet = bulletObj.GetComponent<BulletController>();
                bullet.InitializeEnemyBullet(Vector2.down, shotPattern.defaultBulletSpeed, shotPattern.bulletSprite);
            }
        }*/
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
    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // add death sound,vfx
        gameObject.SetActive(false);
    }
}
