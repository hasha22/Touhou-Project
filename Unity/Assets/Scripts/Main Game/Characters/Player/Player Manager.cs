using KH;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    [Header("Player Data")]
    public PlayableCharacterData characterData;
    [SerializeField] private int currentPlayerLives;
    [SerializeField] private int currentPower;

    // Temporary, to be moved to Audio Manager
    [SerializeField] private AudioClip deathSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy Bullet"))
        {
            BulletController bullet = collision.GetComponent<BulletController>();
            Die();
            ObjectPool.instance.ReturnToPool(collision.gameObject);
        }
    }
    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, transform.position, 1.0f);
        gameObject.SetActive(false);
    }
}
